namespace Opticall.Luxafor;

public interface ILuxaforDevice
{
    void Run(ICommand command);
    void Off();
}
