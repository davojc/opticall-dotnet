using HidSharp;
using Opticall.Luxafor.Commands;

namespace Opticall.Luxafor;

public class LuxaforDevice : ILuxaforDevice
{
    private HidDevice _hidDevice;

    private LuxaforDevice(HidDevice hidDevice) 
    {
        _hidDevice = hidDevice;
    }

    public void Run(ICommand command)
    {
        using(var stream = _hidDevice.Open())
        {
            var data = command.GetCommandBytes();
            Console.WriteLine("Writing to device");

            stream.Write(data, 0, data.Length);
        }
    }

    public void Off()
    {
        using(var stream = _hidDevice.Open())
        {
            var off = new OffCommand();
            var data = off.GetCommandBytes();

            stream.Write(data, 0, data.Length);
        }
    }

    public static ILuxaforDevice Find() 
    {
        var list = DeviceList.Local;

        var allDeviceList = list.GetAllDevices().ToArray();

        foreach (Device dev in allDeviceList)
        {
            var name = dev.GetFriendlyName();

            if (name != null && name.Equals("LUXAFOR FLAG"))
            {
                var hid = dev as HidDevice;

                if(hid != null)
                {
                    return new LuxaforDevice(hid);
                }
            }
        }

        throw new DeviceNotFoundException();
    }
}
