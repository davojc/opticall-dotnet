using System.ComponentModel;

namespace Opticall.Messaging;

public interface IMessageSerialiser<T> where T : struct, IMessage
{
    bool TryDeserialise(byte[] data, out T? message);

    byte[] Serialise(T message);
}


