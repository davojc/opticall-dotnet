using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class FadeCommand() : LedCommand(CommandType.Fade)
{
    protected override byte[] Map(byte[] command, object[] args)
    {
        if (args.Length == 2 && args[0] is string)
        {
            var color = HexToRgb((string)args[0]);

            command[2] = color.R;
            command[3] = color.G;
            command[4] = color.B;
            command[5] = (byte)args[1];
        }
        else if (args.Length == 4)
        {
            command[2] = (byte)args[0];
            command[3] = (byte)args[1];
            command[4] = (byte)args[2];
            command[5] = (byte)args[3];
        }

        return command;
    }
}