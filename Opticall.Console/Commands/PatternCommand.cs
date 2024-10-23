using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class PatternCommand() : Command(CommandType.Pattern)
{
    protected override int Length => 3;

    protected override IEnumerable<ArgumentMap> GetMaps()
    {
        yield return new ArgumentMap(0, 1);
        yield return new ArgumentMap(1, 2);
    }
}