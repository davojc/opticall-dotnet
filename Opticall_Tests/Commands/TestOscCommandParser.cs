using Opticall.Console.Command;
using Opticall.Console.Command.Commands;
using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall_Tests.Commands;


public abstract class OscTesting
{
    protected OscMessage BuildMessage(string address, params object[] args)
    {
        var message = new OscMessage(new Address(address), args);
        return message;
    }
}

public class TestOscCommandParser : OscTesting
{
    [Fact]
    public void Osc_TurnOn()
    {
        var message = BuildMessage("/led/all/on", "color:#FF0000", "time:20");

        var parser = new OscCommandParser();

        var result = parser.Parse(message).ToArray();

        Assert.NotNull(result);

        var commandSerializer = new CommandOscDeserializer();
        var cmd = commandSerializer.Convert<OnCommand>(message, Led.Back);
        var bytes = cmd.ToBytes();
        Assert.NotNull(cmd);
        Assert.NotNull(bytes);
    }
}