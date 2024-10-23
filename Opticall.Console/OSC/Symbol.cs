namespace Opticall.Console.OSC;

public readonly struct Symbol(string value)
{
    public string Value { get; } = value;
}