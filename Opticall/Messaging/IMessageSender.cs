namespace Opticall.Messaging;

public interface IMessageSender<T, M> : IDisposable where M : Enum
{
    Task Send(M messageType, T message);
}