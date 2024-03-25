
using Opticall.Config;
using Opticall.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;

namespace Opticall.IO
{
    public class UdpListener : IMessageListener<Message>
    {
        NetworkSettings _settings;
        UdpClient _client;

        Subject<Message> _subject;

        CancellationTokenSource _cancelTokenSrc;
        private IMessageSerialiser<Message> _serialiser;

        public UdpListener(NetworkSettings settings, IMessageSerialiser<Message> serialiser)
        {
            _settings = settings;
            _subject = new Subject<Message>();
            _client = new UdpClient(new IPEndPoint(IPAddress.Any, _settings.Port));
            _cancelTokenSrc = new CancellationTokenSource();
            _serialiser = serialiser;
        }

        public void Dispose()
        {
            //_cancelTokenSrc.Cancel(false);
            _cancelTokenSrc.Dispose();
            _client.Close();
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

                    if (_serialiser.TryDeserialise(result.Buffer, out Message? msg))
                    {
                        Console.WriteLine("Deserialised");
                        if(msg.HasValue)
                        {
                            _subject.OnNext(msg.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not deserialise message.");
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
        }

        public IDisposable Subscribe(IObserver<Message> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}