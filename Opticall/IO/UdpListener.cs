
using Opticall.Config;
using Opticall.Messaging;
using Opticall.Messaging.Signals;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;

namespace Opticall.IO;

public class UdpListener<T, M> : ITopicListener<T, M> where M : struct, Enum
{
    private Encoding _encoding;
    NetworkSettings _settings;
    UdpClient _client;

    Subject<Tuple<T,M>> _subject;

    CancellationTokenSource _cancelTokenSrc;
    private IContentSerialiser<T, M> _contentSerializer;

    private bool _disposed;

    public UdpListener(NetworkSettings settings, IContentSerialiser<T, M> contentSerializer) : this(settings, contentSerializer, Encoding.UTF8)
    {
    }

    public UdpListener(NetworkSettings settings, IContentSerialiser<T, M> contentSerializer, Encoding encoding)
    {
        _encoding = encoding;
        _settings = settings;
        _subject = new Subject<Tuple<T,M>>();
        _client = new UdpClient(new IPEndPoint(IPAddress.Any, _settings.Port));
        _cancelTokenSrc = new CancellationTokenSource();
        _contentSerializer = contentSerializer;
    }

    ~UdpListener() // the finalizer
    {
        Dispose(false);
    }

    public void Stop() 
    {
        _cancelTokenSrc.Cancel();
        Console.WriteLine("Stop called");
    }

    public async Task StartListening()
    {
        try
        {
            while (!_cancelTokenSrc.Token.IsCancellationRequested)
            {
                UdpReceiveResult result = await _client.ReceiveAsync(_cancelTokenSrc.Token);

                var message = _encoding.GetString(result.Buffer);

                var split = message.Split('|');

                if(split.Length != 3)
                    continue;

                var topic = split[0];

                if(!string.Equals(topic, Topic.GetTopic<T>(), StringComparison.OrdinalIgnoreCase))
                    continue;

                if(!Enum.TryParse<M>(split[1], true, out M contentType))
                {
                    Console.WriteLine("Did not recognise contentType: " + split[1]);
                    continue;
                }

                var content = _contentSerializer.Deserialise(contentType, split[2]);

                if(content != null)
                {
                    _subject.OnNext(new Tuple<T, M>(content, contentType));
                }
                else
                {
                    Console.WriteLine("Could not deserialise content.");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested, do nothing
            Console.WriteLine("Finished");
            _subject.OnCompleted();
        }
        catch (SocketException ex)
        {
            _subject.OnError(ex);
            Console.WriteLine($"SocketException: {ex.Message}");
        }
        catch(Exception e)
        {
            _subject.OnError(e);
        }
    }

    public IDisposable Subscribe(IObserver<Tuple<T, M>> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _cancelTokenSrc.Dispose();
                _client.Close();
            }
            // Release unmanaged resources.
            // Set large fields to null.                
            _disposed = true;
        }
    }
}
