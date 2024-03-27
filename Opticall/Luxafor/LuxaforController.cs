using Opticall.Messaging;
using Opticall.Messaging.Signals;

namespace Opticall.Luxafor;
public class LuxaforController : IObserver<Tuple<ISignalTopic, SignalType>>
{
    private ILuxaforDevice _luxaforDevice;
    private ICommandBuilder _commandBuilder;

    public LuxaforController(ILuxaforDevice luxaforDevice, ICommandBuilder commandBuilder)
    {
        _luxaforDevice = luxaforDevice;
        _commandBuilder = commandBuilder;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Tuple<ISignalTopic, SignalType> value)
    {
        var command = _commandBuilder.Build(value.Item1);

        _luxaforDevice.Run(command);
    }
}
