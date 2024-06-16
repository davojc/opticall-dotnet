using Opticall.Luxafor;
using Opticall.Messaging.Signals;

namespace Opticall_Tests.Messaging.Requests.Signals;


public class UnitTestSignalSerialisation
{
    [Theory]
    [MemberData(nameof(GetInputData))]
    public void SerialiseSignals(SignalType signalType, ISignalTopic signal) 
    {
        var serialiser = new JsonSignalSerialiser();

        var serialisedString = serialiser.Serialise(signal);
        var deserialised = serialiser.Deserialise(signalType, serialisedString);

        Assert.NotNull(deserialised);
        Assert.False(Object.ReferenceEquals(signal, deserialised));
        Assert.True(signal.Equals(deserialised), "Deserialised signal does not match input signal.");
    }

    public static IEnumerable<object[]> GetInputData()
    {
        yield return new object[] { SignalType.Fade, new FadeSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35, Speed = 12 } };
        yield return new object[] { SignalType.Off, new OffSignal { Target = "thetarget" } };
        yield return new object[] { SignalType.On, new OnSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35 } };
        yield return new object[] { SignalType.Pattern, new PatternSignal { Target = "thetarget", Type = PatternType.Police, Repeat = 10 } };
        yield return new object[] { SignalType.Strobe, new StrobeSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35, Speed = 12, Repeat = 41 } };
        yield return new object[] { SignalType.Wave, new WaveSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35, Speed = 12, Repeat = 72 } };
    }
}

public class UnitTestSignalToCommand
{
    [Theory]
    [MemberData(nameof(GetInputData))]
    public void CreateCommand(ISignalTopic signal, byte[] expectedCommand)
    {
        var commandBuilder = new CommandBuilder();

        var command = commandBuilder.Build(signal);

        Assert.NotNull(command);

        Assert.Equal(expectedCommand.Length, command.Length);

        for(var i = 0; i < expectedCommand.Length; i++)
        {
            Assert.Equal(expectedCommand[i], command[i]);
        }
    }

    public static IEnumerable<object[]> GetInputData()
    {
        yield return new object[] { new FadeSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35, Speed = 12 }, new byte[] { 2, 0xff, 255, 20, 35, 12, 0, 0 } };
        yield return new object[] { new OffSignal { Target = "thetarget" }, new byte[] { 1, 0xff, 0, 0, 0, 0, 0, 0 } };
        yield return new object[] { new OnSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35 }, new byte[] { 1, 0xff, 255, 20, 35, 0, 0, 0 } };
        yield return new object[] { new PatternSignal { Target = "thetarget", Type = PatternType.Police, Repeat = 10 }, new byte[] { 6, 5, 10} };
        yield return new object[] { new StrobeSignal { Target = "thetarget", Red = 255, Green = 20, Blue = 35, Speed = 12, Repeat = 41 }, new byte[] { 3, 0xff, 255, 20, 35, 12, 0, 41 } };
        yield return new object[] { new WaveSignal { Target = "thetarget", Type = WaveType.Short, Red = 255, Green = 20, Blue = 35, Speed = 12, Repeat = 72 }, new byte[] { 4, 1, 255, 20, 35, 0, 72, 12 } };
    }
}