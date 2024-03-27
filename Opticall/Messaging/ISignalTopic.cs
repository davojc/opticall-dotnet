namespace Opticall.Messaging;

[Topic("signal")]
public interface ISignalTopic
{
    string? Target { get; }
}
