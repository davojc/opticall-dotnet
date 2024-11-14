using Opticall.Console.OSC;

namespace Opticall.Console.Command;

public interface ICommandListener : IDisposable, IObservable<OscMessage>
{
    Task StartListening(int port, CancellationToken cancellation);
}
