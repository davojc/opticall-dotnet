namespace Opticall.Messaging.Signals;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SignalAttribute(SignalType signalType) : Attribute
{
    public SignalType Signal { get; private set; } = signalType;
}
