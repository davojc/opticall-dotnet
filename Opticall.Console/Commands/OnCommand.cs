using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class OnCommand() : LedCommand(CommandType.Color)
{
    protected override byte[] Map(byte[] command, object[] args)
    {
        if (args.Length == 1 && args[0] is string)
        {
            var color = HexToRgb((string)args[0]);

            command[2] = color.R;
            command[3] = color.G;
            command[4] = color.B;
        }
        else if (args.Length == 3)
        {
            command[2] = (byte)args[0];
            command[3] = (byte)args[1];
            command[4] = (byte)args[2];
        }

        return command;
    }
}