namespace Opticall.Luxafor.Commands;

public class FadeCommand : Command
{
    private byte? _red, _green, _blue, _speed;

    public FadeCommand(Led led, byte? speed, byte? red, byte? green, byte? blue) : base(CommandType.Fade, (byte) led)
    {
        _red = red;
        _green = green;
        _blue = blue;

        _speed = speed;
    }

    protected override byte? Position1 { get { return _red; } }

    protected override byte? Position2 { get { return _green; } }

    protected override byte? Position3 { get { return _blue; } }

    protected override byte? Position4 { get { return _speed; } }

    public override bool IsValid()
    {
        return Position1.HasValue && Position2.HasValue && Position3.HasValue && Position4.HasValue;
    }
}
