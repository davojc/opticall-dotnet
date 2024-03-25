namespace Opticall.Messaging
{
    public interface IMessageSender<T> : IDisposable where T: IMessage
    {
        Task Send(T message);
    }
}