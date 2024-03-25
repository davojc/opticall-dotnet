using Opticall.Luxafor.Commands;
using Opticall.Messaging;

namespace Opticall.Luxafor;
public class LuxaforController : IObserver<Message>
{
    private ILuxaforDevice _luxaforDevice;

    public LuxaforController(ILuxaforDevice luxaforDevice)
    {
        _luxaforDevice = luxaforDevice;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Message value)
    {
        ICommand command;

        switch(value.Request)
        {
            case RequestType.On:
                command = new ColorCommand(Led.All, value.Red, value.Green, value.Blue);
                break;

            case RequestType.Fade:
                command = new FadeCommand(Led.All, value.Speed, value.Red, value.Green, value.Blue);
                break;

            case RequestType.Flash:
                command = new StrobeCommand(Led.All, value.Speed, value.Repeat, value.Red, value.Green, value.Blue);
                break;

            case RequestType.Wave:
                command = new WaveCommand(Led.All, value.Speed, value.Repeat, value.Red, value.Green, value.Blue);
                break;

            case RequestType.Pattern:

                Console.WriteLine(value.Pattern);

                if(Enum.TryParse<PatternType>(value.Pattern, true, out PatternType patternType))
                    {
                        command = new PatternCommand(patternType, value.Repeat);
                        Console.WriteLine("Created pattern command");
                    }
                else
                    throw new ArgumentException("Cannot parse patterntype");
                
                break;

            default:
                command = new OffCommand();
                break;
        }

        _luxaforDevice.Run(command);
    }
}
