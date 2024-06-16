
using Microsoft.Extensions.Logging;
using Opticall.Config;
using Opticall.Messaging;
using Opticall.Messaging.Signals;
using System.Net.Sockets;
using System.Text;

namespace Opticall.IO
{
    public class UdpLogger<T, M> : ILogger
    {
        private Encoding _encoding;
        NetworkSettings _settings;
        UdpClient _client;

        public UdpLogger(NetworkSettings settings)
        {
            _settings = settings;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {


            throw new NotImplementedException();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }
    }

    public class UdpSender<T, M> : IMessageSender<T, M> where M : struct, Enum
    {
        private Encoding _encoding;
        NetworkSettings _settings;
        UdpClient _client;
        IContentSerialiser<T, M> _serialiser;

        public UdpSender(NetworkSettings settings, IContentSerialiser<T, M> serialiser) : this(settings, serialiser, Encoding.UTF8)
        {
        }

        public UdpSender(NetworkSettings settings, IContentSerialiser<T, M> serialiser, Encoding encoding)
        {
            _encoding = encoding;
            _settings = settings;
            _client = new UdpClient();
            _serialiser = serialiser;
        }

        public async Task Send(M messageType, T message)
        {
            var signalString = _serialiser.Serialise(message);
            
            var topic = Topic.GetTopic<ISignalTopic>();
            
            var sb = new StringBuilder();
            sb.Append(topic);
            sb.Append('|');
            sb.Append(messageType.ToString().ToLowerInvariant());
            sb.Append('|');
            sb.Append(signalString);

            var data = _encoding.GetBytes(sb.ToString());

            Console.WriteLine("Sending message");
            await _client.SendAsync(data, data.Length, _settings.BindingAddress, _settings.Port);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}