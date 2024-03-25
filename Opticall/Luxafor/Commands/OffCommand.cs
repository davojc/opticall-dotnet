
namespace Opticall.Luxafor.Commands;

public class OffCommand : Command
{
    public OffCommand() : base(CommandType.Color, (byte) Led.All)
    {
    }

    public override bool IsValid()
    {
        return true;
    }
}
