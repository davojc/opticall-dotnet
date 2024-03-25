
namespace Opticall.Luxafor.Commands;

public class StrobeCommand : Command
{
    private byte? _red, _green, _blue, _speed, _repeat;

    public StrobeCommand(Led led, byte? speed, byte? repeat, byte? red, byte? green, byte? blue) : base(CommandType.Strobe, (byte) led)
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

    protected override byte? Position4 { get { return _speed; } }

    protected override byte? Position6 { get { return _repeat; } }

    public override bool IsValid()
    {
        return Position1.HasValue && Position2.HasValue && Position3.HasValue && Position4.HasValue && Position6.HasValue;
    }
}
