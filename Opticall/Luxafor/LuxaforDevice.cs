using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using HidSharp;

namespace Opticall.Luxafor;

public class LuxaforDevice : ILuxaforDevice
{
    private HidDevice _hidDevice;

    private LuxaforDevice(HidDevice hidDevice) 
    {
        _hidDevice = hidDevice;
    }

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
            Console.WriteLine("Writing to device");
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
            Console.WriteLine("Writing to device");
            deviceStream.Write(command, 0, command.Length);
            deviceStream.Close();
        }
        else 
        {
            Console.WriteLine("Couldn't open stream;");
        }
      
    }

    public static ILuxaforDevice Find() 
    {
        var list = DeviceList.Local;

        var allDeviceList = list.GetAllDevices().ToArray();

        HashSet<string> names = new HashSet<string>();

        foreach (Device dev in allDeviceList)
        {
            var name = dev.GetFriendlyName();

            var hid = dev as HidDevice;

            if(hid == null)
            {
                continue;    
            }

            names.Add(hid.DevicePath);

            if (name != null && name.Equals("LUXAFOR FLAG"))
            {
                return new LuxaforDevice(hid);
            }
        }

        Console.WriteLine("Vendor Id: " + Convert.ToInt64(0x04d8));
        Console.WriteLine("Other Id: " + Convert.ToInt64(0xf372));

        Console.WriteLine(string.Join("\n", names));

        throw new DeviceNotFoundException();
    }
}
