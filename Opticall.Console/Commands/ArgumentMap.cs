namespace Opticall.Console.Commands;

public readonly struct ArgumentMap(int from, int to)
{
    public int From { get; } = from;
    public int To { get; } = to;
}