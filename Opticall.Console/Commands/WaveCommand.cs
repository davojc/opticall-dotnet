using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class WaveCommand() : Command(CommandType.Wave)
{
    protected override int Length => 8;

    protected override IEnumerable<ArgumentMap> GetMaps()
    {
        yield return new ArgumentMap(0, 1);
        yield return new ArgumentMap(1, 2);
        yield return new ArgumentMap(2, 3);
        yield return new ArgumentMap(3, 4);
        yield return new ArgumentMap(4, 7);
        yield return new ArgumentMap(5, 6);
    }
}