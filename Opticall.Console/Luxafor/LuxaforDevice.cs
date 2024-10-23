using System.Runtime.InteropServices;
using System.Security.Cryptography;
using HidSharp;

namespace Opticall.Console.Luxafor;

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

    public static Task<ILuxaforDevice> FindAsync(CancellationTokenSource cancellationTokenSource)
    {
        System.Console.WriteLine("Searching for device...");

        return Task<ILuxaforDevice>.Factory.StartNew(() =>
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var list = DeviceList.Local;

                var allDeviceList = list.GetAllDevices().ToArray();

                var names = new HashSet<string>();

                var deviceManager = new LuxaforDeviceManager();

                foreach (Device dev in allDeviceList)
                {
                    var name = dev.GetFriendlyName();

                    var hid = dev as HidDevice;

                    if (hid == null)
                    {
                        continue;
                    }

                    names.Add(hid.DevicePath);

                    if (name != null && name.Equals("LUXAFOR FLAG"))
                    {
                        System.Console.WriteLine("Found device.");

                        deviceManager.Add(new LuxaforDevice(hid));
                    }
                }

                if (deviceManager.Count > 0)
                {
                    return deviceManager;
                }

                System.Console.WriteLine("Still waiting for device to be plugged in...");
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }

            return null;
        }, cancellationTokenSource.Token);
    }
}
