namespace Opticall.Messaging.Signals;

[Topic("config")]
public interface IConfigTopic
{
    string? Target { get; }
}
