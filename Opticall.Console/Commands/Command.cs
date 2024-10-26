using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public abstract class Command(CommandType commandType)
{
    protected abstract int Length { get; }

    protected virtual int CommandPosition => 0;

    public virtual byte[] CreateCommand(OscMessage message)
    {
        var command = BuildCommandTemplate();
        var args = message.Arguments?.ToArray();

        if (args != null)
        {
            Map(command, args);
        }

        return command;
    }

    protected byte[] BuildCommandTemplate()
    {
        var command = Enumerable.Repeat((byte)0, Length).ToArray();

        command[CommandPosition] = (byte)commandType;

        return command;
    }

    protected abstract byte[] Map(byte[] command, object[] args);

    protected (byte R, byte G, byte B) HexToRgb(string hexCode)
    {
        // Remove the leading '#' if present
        hexCode = hexCode.TrimStart('#');

        // Parse each color component from hexadecimal to integer
        int r = int.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return ((byte)r, (byte)g, (byte)b);
    }
}