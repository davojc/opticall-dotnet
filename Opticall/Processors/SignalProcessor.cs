using Opticall.Luxafor;
using Opticall.Messaging.Signals;

namespace Opticall.Processors;
public class SignalProcessor : IObserver<Tuple<ISignalTopic, SignalType>>
{
    private ILuxaforDevice _luxaforDevice;
    private ICommandBuilder _commandBuilder;
    private string _name;
    private string _group;

    public SignalProcessor(ILuxaforDevice luxaforDevice, ICommandBuilder commandBuilder, string name, string group)
    {
        _luxaforDevice = luxaforDevice;
        _commandBuilder = commandBuilder;
        _name = name;
        _group = group;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Tuple<ISignalTopic, SignalType> value)
    {
        if (string.Equals(value.Item1.Target, _name) || string.Equals(value.Item1.Target, _group))
        {
            var command = _commandBuilder.Build(value.Item1);
            _luxaforDevice.Run(command);
        }
    }
}
