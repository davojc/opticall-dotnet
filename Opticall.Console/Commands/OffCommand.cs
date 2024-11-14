using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public class OffCommand() : LedCommand(CommandType.Color)
{
    protected override byte[] Map(byte[] command, object[] args)
    {
        return command;
    }
}