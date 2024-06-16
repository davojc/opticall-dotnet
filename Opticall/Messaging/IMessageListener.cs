namespace Opticall.Messaging;

public interface IMessageListener<T, M> : IDisposable, IObservable<Tuple<T, M>> where M : struct, Enum
{
    Task StartListening();
}
