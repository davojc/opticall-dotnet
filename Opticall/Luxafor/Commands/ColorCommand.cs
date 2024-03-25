
namespace Opticall.Luxafor.Commands;

public class ColorCommand : Command
{
    private byte? _red, _green, _blue;

    public ColorCommand(Led led, byte? red, byte? green, byte? blue) : base(CommandType.Color, (byte) led)
    {
        _red = red;
        _green = green;
        _blue = blue;
    }

    protected override byte? Position1 { get { return _red; } }

    protected override byte? Position2 { get { return _green; } }

    protected override byte? Position3 { get { return _blue; } }

    public override bool IsValid()
    {
        return Position1.HasValue && Position2.HasValue && Position3.HasValue;
    }
}
