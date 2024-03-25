
using Opticall.Config;
using Opticall.Messaging;
using System.Net.Sockets;

namespace Opticall.IO
{
    public class UdpSender<T> : IMessageSender<T> where T : struct, IMessage
    {
        NetworkSettings _settings;
        UdpClient _client;
        IMessageSerialiser<T> _serialiser;

        public UdpSender(NetworkSettings settings, IMessageSerialiser<T> serialiser)
        {
            _settings = settings;
            _client = new UdpClient();
            _serialiser = serialiser;
        }

        public async Task Send(T message)
        {
            byte[] data = _serialiser.Serialise(message);
            await _client.SendAsync(data, data.Length, _settings.BindingAddress, _settings.Port);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}