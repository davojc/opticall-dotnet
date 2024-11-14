using Opticall.Console.Command;

namespace Opticall.Console.Luxafor;

public interface ILuxaforDevice
{   
    void Run(ICommand command);
}
