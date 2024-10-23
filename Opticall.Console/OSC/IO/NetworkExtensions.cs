using Opticall.Console.OSC.Converters;
using System.Net.Sockets;

namespace Opticall.Console.OSC.IO
{
    public static class NetworkExtensions
    {
        private static readonly BytesConverter BytesConverter = new BytesConverter();
        private static readonly OscMessageConverter MessageConverter = new OscMessageConverter();

        public static async Task SendMessageAsync(this UdpClient client, OscMessage message)
        {
            var dWords = MessageConverter.Serialize(message);
            _ = BytesConverter.Deserialize(dWords, out var bytes);
            var byteArray = bytes.ToArray();
            await client.SendAsync(byteArray, byteArray.Length);
        }

        public static async Task<OscMessage> ReceiveMessageAsync(this UdpClient client, CancellationToken cancelToken)
        {
            var receiveResult = await client.ReceiveAsync(cancelToken);
            var dWords = BytesConverter.Serialize(receiveResult.Buffer);
            MessageConverter.Deserialize(dWords, out var value);
            return value;
        }
    }
}
