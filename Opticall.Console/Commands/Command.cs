using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public abstract class Command(CommandType commandType)
{
    protected abstract int Length { get; }

    public byte[] CreateCommand(OscMessage message)
    {
        var command = Enumerable.Repeat((byte)0, Length).ToArray();

        command[0] = (byte)commandType;

        var args = message.Arguments?.ToArray();

        if (args != null)
        {
            foreach (var map in GetMaps())
            {
                command[map.To] = Convert.ToByte(args[map.From]);
            }
        }

        return command;
    }

    protected abstract IEnumerable<ArgumentMap> GetMaps();
}