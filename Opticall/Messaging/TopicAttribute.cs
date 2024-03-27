namespace Opticall.Messaging;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class TopicAttribute(string topic) : Attribute
{
    public string Topic { get; private set; } = topic;
}