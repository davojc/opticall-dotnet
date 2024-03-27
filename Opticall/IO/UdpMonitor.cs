
using Opticall.Config;
using Opticall.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;

namespace Opticall.IO;

public class UdpMonitor
{
    NetworkSettings _settings;
    UdpClient _client;

    CancellationTokenSource _cancelTokenSrc;

    private bool _disposed;

    public UdpMonitor(NetworkSettings settings)
    {
        _settings = settings;
         _client = new UdpClient(new IPEndPoint(IPAddress.Any, _settings.Port));
        _cancelTokenSrc = new CancellationTokenSource();
     }

    ~UdpMonitor() // the finalizer
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


/*
                if (_requestSerializer.TryDeserialise(result.Buffer, out ISignalRequest request))
                {
                        _subject.OnNext(request);
                }
                else
                {
                    Console.WriteLine("Could not deserialise message.");
                }*/
            }
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested, do nothing
            Console.WriteLine("Finished");
            //_subject.OnCompleted();
        }
        catch (SocketException ex)
        {
            //_subject.OnError(ex);
            Console.WriteLine($"SocketException: {ex.Message}");
        }
    }

    //public IDisposable Subscribe(IObserver<ISignalRequest> observer)
    //{
    //   return _subject.Subscribe(observer);
    //}

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
