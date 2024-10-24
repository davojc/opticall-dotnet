using System.Runtime.InteropServices;
using System.Security.Cryptography;
using HidSharp;

namespace Opticall.Console.Luxafor;

public class LuxaforDevice : ILuxaforDevice
{
    private readonly HidDevice _hidDevice;

    public LuxaforDevice(HidDevice hidDevice) 
    {
        _hidDevice = hidDevice;
    }

    public string DeviceId => _hidDevice.DevicePath;

    public void Run(byte[]? command)
    {
        if(command == null)
            return;

        // For some reason array needs to be shifted by 1 for windows.
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Array.Resize(ref command, command.Length + 1);
            Array.Copy(command, 0, command, 1, command.Length -1 );
            command[0] = 0;
        }

        using(var stream = _hidDevice.Open())
        {
            System.Console.WriteLine("Writing to device");
            stream.Write(command, 0, command.Length);
        }
    }

    public void RunDirect(byte[] command)
    {
        if(command == null)
            return;

        var writeData = Array.ConvertAll(command, x => x);


        if(_hidDevice.TryOpen(out DeviceStream deviceStream))
        {
            System.Console.WriteLine("Writing to device");
            deviceStream.Write(command, 0, command.Length);
            deviceStream.Close();
        }
        else 
        {
            System.Console.WriteLine("Couldn't open stream;");
        }
    }
}
