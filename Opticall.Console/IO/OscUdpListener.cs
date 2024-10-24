using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using Opticall.Console.Commands;
using Opticall.Console.OSC;
using Opticall.Console.OSC.IO;

namespace Opticall.Console.IO;

public class OscUdpListener : ICommandListener
{
    readonly UdpClient _client;
    readonly Subject<OscMessage> _subject;
    readonly CancellationToken _cancelTokenSrc;

    private bool _disposed;

    public OscUdpListener(int port, CancellationToken cancellation)
    {
        _subject = new Subject<OscMessage>();

        var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        var ipEndpoint = new IPEndPoint(IPAddress.Any, port);
        udpSocket.Bind(ipEndpoint);

        _client = new UdpClient() { Client = udpSocket };
        _cancelTokenSrc = cancellation;
    }

    ~OscUdpListener() // the finalizer
    {
        Dispose(false);
    }

    public async Task StartListening()
    {
        try
        {
            while (!_cancelTokenSrc.IsCancellationRequested)
            {
                var message = await _client.ReceiveMessageAsync(_cancelTokenSrc);
                _subject.OnNext(message);
            }
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested, do nothing
            System.Console.WriteLine("Finished");
            _subject.OnCompleted();
        }
        catch (SocketException ex)
        {
            _subject.OnError(ex);
            System.Console.WriteLine($"SocketException: {ex.Message}");
        }
        catch(Exception e)
        {
            _subject.OnError(e);
        }
    }

    public IDisposable Subscribe(IObserver<OscMessage> observer)
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
                _client.Close();
            }
            // Release unmanaged resources.
            // Set large fields to null.                
            _disposed = true;
        }
    }
}
