using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public abstract class LedCommand(CommandType commandType) : Command(commandType)
{
    protected override int Length => 8;
    public byte[] CreateCommand(Led led, OscMessage message)
    {
        var command = BuildCommandTemplate();
        var args = message.Arguments?.ToArray();

        command[1] = (byte)led;

        if (args != null)
        {
            command = Map(command, args);
        }

        return command;
    }


}