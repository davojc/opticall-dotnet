namespace Opticall.Messaging.Signals;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class CommandFieldAttribute(byte position) : Attribute
{
    public byte Position { get; private set; } = position;
}
