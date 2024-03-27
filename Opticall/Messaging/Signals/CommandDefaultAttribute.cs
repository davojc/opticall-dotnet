namespace Opticall.Messaging.Signals;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class CommandDefaultAttribute(byte position, byte value) : Attribute
{
    public byte Position { get; private set; } = position;
    public byte Value { get; private set; } = value;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CommandTemplateAttribute(params byte[] template) : Attribute
{
    public byte[] Template { get; private set; } = [.. template];
}
