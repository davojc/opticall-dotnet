namespace Opticall.Console.OSC;

public readonly struct Address(string value)
{
    public string Value { get; } = value;
}