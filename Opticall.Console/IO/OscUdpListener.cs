using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command;
using Opticall.Console.OSC;
using Opticall.Console.OSC.IO;

namespace Opticall.Console.IO;

public class OscUdpListener : ICommandListener
{
    private readonly ILogger<OscUdpListener> _logger;
    readonly Subject<OscMessage> _subject;
    UdpClient _client;
    CancellationToken _cancellation;

    private bool _disposed;

    public OscUdpListener(ILogger<OscUdpListener> logger)
    {
        _logger = logger;
        _subject = new Subject<OscMessage>();
    }

    ~OscUdpListener() // the finalizer
    {
        Dispose(false);
    }

    public async Task StartListening(int port, CancellationToken cancellation)
    {
        _logger.LogInformation($"Starting listening on port: {port}");
        var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        var ipEndpoint = new IPEndPoint(IPAddress.Any, port);
        udpSocket.Bind(ipEndpoint);
        _cancellation = cancellation;

        _client = new UdpClient() { Client = udpSocket };

        try
        {
            while (!_cancellation.IsCancellationRequested)
            {
                var message = await _client.ReceiveMessageAsync(_cancellation);
                _subject.OnNext(message);
            }
        }
        catch (OperationCanceledException)
        {
            // Cancellation requested, do nothing
            _logger.LogInformation("Shutting down listener.");
            _subject.OnCompleted();
        }
        catch (SocketException ex)
        {
            _subject.OnError(ex);
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
