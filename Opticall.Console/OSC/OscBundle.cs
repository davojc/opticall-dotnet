namespace Opticall.Console.OSC;

public readonly struct OscBundle(Timetag timetag, IEnumerable<OscMessage> messages)
{
    public Timetag Timetag { get; } = timetag;

    public IEnumerable<OscMessage> Messages { get; } = messages;
}