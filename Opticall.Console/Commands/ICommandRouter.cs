using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public interface ICommandRouter : IObserver<OscMessage>
{
    void AddRoute(Command command, Action<byte[]> onRoute, string route, params string?[] alternates);
}