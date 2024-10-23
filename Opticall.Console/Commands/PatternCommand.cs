using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class PatternCommand() : Command(CommandType.Pattern)
{
    protected override int Length => 4;

    protected override int CommandPosition => 1;

    protected override IEnumerable<ArgumentMap> GetMaps()
    {
        yield return new ArgumentMap(0, 2);
        yield return new ArgumentMap(1, 3);
    }
}