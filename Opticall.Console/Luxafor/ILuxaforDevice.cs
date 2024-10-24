namespace Opticall.Console.Luxafor;

public interface ILuxaforDevice
{   
    void Run(byte[]? command);

    void RunDirect(byte[] command);
}
