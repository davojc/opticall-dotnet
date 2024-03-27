namespace Opticall.Messaging.Signals;

public interface ICommandBuilder
{
    byte[]? Build(ISignalTopic signal);
}
