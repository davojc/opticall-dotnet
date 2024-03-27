namespace Opticall.Messaging;

public interface ITopicListener<T, M> : IDisposable, IObservable<Tuple<T, M>> where M : struct, Enum
{
    Task StartListening();
}
