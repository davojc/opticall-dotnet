namespace Opticall.Luxafor.Commands;

public class PatternCommand : Command
{
    private byte? _repeat;

    public PatternCommand(PatternType patternType, byte? repeat) : base(CommandType.Pattern, (byte) patternType)
    {
        _repeat = repeat;
    }

    protected override byte? Position1 { get { return _repeat; } }

    public override bool IsValid()
    {
        return Position1.HasValue;
    }
}
