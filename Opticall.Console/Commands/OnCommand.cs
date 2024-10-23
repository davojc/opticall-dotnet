using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class OnCommand() : Command(CommandType.Color)
{
    protected override int Length => 8;

    protected override IEnumerable<ArgumentMap> GetMaps()
    {
        yield return new ArgumentMap(0, 1);
        yield return new ArgumentMap(1, 2);
        yield return new ArgumentMap(2, 3);
        yield return new ArgumentMap(3, 4);
    }
}