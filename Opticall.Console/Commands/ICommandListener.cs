using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public interface ICommandListener : IDisposable, IObservable<OscMessage>
{
    Task StartListening(int port, CancellationToken cancellation);
}
