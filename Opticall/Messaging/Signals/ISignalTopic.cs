namespace Opticall.Messaging.Signals;

[Topic("signal")]
public interface ISignalTopic
{
    string? Target { get; }
}
