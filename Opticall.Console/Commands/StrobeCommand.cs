using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class StrobeCommand() : LedCommand(CommandType.Strobe)
{
    protected override byte[] Map(byte[] command, object[] args)
    {
        if (args.Length == 1 && args[0] is string)
        {
            var color = HexToRgb((string)args[0]);

            command[2] = color.R;
            command[3] = color.G;
            command[4] = color.B;
            command[5] = (byte)args[1];
            command[7] = (byte)args[2];
        }
        else if (args.Length == 3)
        {
            command[2] = (byte)args[0];
            command[3] = (byte)args[1];
            command[4] = (byte)args[2];
            command[5] = (byte)args[3];
            command[7] = (byte)args[4];
        }

        return command;
    }
}