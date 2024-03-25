namespace Opticall.Messaging
{
    public interface IMessageListener<T> : IDisposable, IObservable<T> where T : struct, IMessage
    {
        Task StartListening();
    }
}