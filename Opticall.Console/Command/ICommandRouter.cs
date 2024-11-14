using Opticall.Console.OSC;

namespace Opticall.Console.Command;

public interface ICommandRouter : IObserver<OscMessage>
{
    void AddIdentifier(string identifier);

    void ReplaceIdentifier(string identifier, string newIdentifier);

    void AddRoute(string route, Action<OscMessage> onRoute);
}