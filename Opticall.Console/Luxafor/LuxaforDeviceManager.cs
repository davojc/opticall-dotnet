namespace Opticall.Console.Luxafor;

public class LuxaforDeviceManager : ILuxaforDevice
{
    private IList<LuxaforDevice> _devices = new List<LuxaforDevice>();

    public int Count => _devices.Count;

    public void Add(LuxaforDevice device)
    {
        _devices.Add(device);
    }

    public void Run(byte[]? command)
    {
        foreach (var device in _devices)
        {
            device.Run(command);
        }
    }

    public void RunDirect(byte[] command)
    {
        foreach (var device in _devices)
        {
            device.RunDirect(command);
        }
    }
}