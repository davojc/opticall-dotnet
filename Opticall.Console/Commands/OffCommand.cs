using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class OffCommand() : Command(CommandType.Color)
{
    protected override int Length => 8;

    protected override IEnumerable<ArgumentMap> GetMaps()
    {
        yield return new ArgumentMap(0, 1);
    }
}