namespace Opticall.Luxafor.Commands;

public class WaveCommand : Command
{
    private byte? _red, _green, _blue, _speed, _repeat;

    public WaveCommand(Led led, byte? speed, byte? repeat, byte? red, byte? green, byte? blue) : base(CommandType.Color, (byte) led)
    {
        _red = red;
        _green = green;
        _blue = blue;

        _speed = speed;
        _repeat = repeat;
    }

    protected override byte? Position1 { get { return _red; } }

    protected override byte? Position2 { get { return _green; } }

    protected override byte? Position3 { get { return _blue; } }

    protected override byte? Position5 { get { return _repeat; } }

    protected override byte? Position6 { get { return _speed; } }

    public override bool IsValid()
    {
        return Position1.HasValue && Position2.HasValue && Position3.HasValue && Position5.HasValue && Position6.HasValue;
    }
}