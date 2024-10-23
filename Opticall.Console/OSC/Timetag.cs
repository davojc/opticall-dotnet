namespace Opticall.Console.OSC;

public readonly struct Timetag(ulong tag)
{
    public ulong Tag { get; } = tag;

    public static Timetag FromDateTime(DateTime value)
    {
        return new Timetag(Utils.DateTimeToTimetag(value));
    }
}