using Opticall.Console.Luxafor;

namespace Opticall.Console.Commands;

public class WaveCommand() : Command(CommandType.Wave)
{
    protected override int Length => 8;
    protected override byte[] Map(byte[] command, object[] args)
    {
        command[1] = Convert.ToByte(args[0]);
        command[2] = (byte)args[1];
        command[3] = (byte)args[2];
        command[4] = (byte)args[3];
        command[7] = (byte)args[4];
        command[6] = (byte)args[5];

        return command;
    }
}