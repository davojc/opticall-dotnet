using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public class PatternCommand() : Command(CommandType.Pattern)
{
    protected override int Length => 4;

    protected override int CommandPosition => 1;
    protected override byte[] Map(byte[] command, object[] args)
    {
        command[2] = Convert.ToByte(args[0]);
        command[3] = Convert.ToByte(args[1]);

        return command;
    }
}