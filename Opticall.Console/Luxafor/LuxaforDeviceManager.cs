using HidSharp;
using System.Threading;

namespace Opticall.Console.Luxafor;

public class LuxaforDeviceManager : ILuxaforDevice
{
    private readonly IDictionary<string, LuxaforDevice> _devices = new Dictionary<string, LuxaforDevice>();
    private readonly DeviceList _deviceList;

    public LuxaforDeviceManager()
    {
        _deviceList = DeviceList.Local;
        _deviceList.Changed += DeviceListChanged;
        RefreshDevices();
    }

    public void Run(byte[]? command)
    {
        foreach (var device in _devices)
        {
            device.Value.Run(command);
        }
    }

    public void RunDirect(byte[] command)
    {
        foreach (var device in _devices)
        {
            device.Value.RunDirect(command);
        }
    }

    private void DeviceListChanged(object? sender, DeviceListChangedEventArgs e)
    {
        RefreshDevices();
    }

    private void RefreshDevices()
    {
        var allDeviceList = _deviceList.GetAllDevices().ToArray();
        var found = new HashSet<string>();

        foreach (Device dev in allDeviceList)
        {
            var name = dev.GetFriendlyName();

            var hid = dev as HidDevice;

            if (hid == null)
            {
                continue;
            }

            if (name is not "LUXAFOR FLAG") continue;

            if (_devices.ContainsKey(hid.DevicePath)) continue;

            System.Console.WriteLine("Adding device.");
            var luxaforDevice = new LuxaforDevice(hid);
            _devices.Add(hid.DevicePath, luxaforDevice);
            found.Add(hid.DevicePath);

           // Thread.Sleep(5);
            //luxaforDevice.RunDirect(new byte[] { 0, (byte)CommandType.Pattern, 1, 5 });
        }

        var notFound = new HashSet<string>();

        foreach (var key in _devices.Keys)
        {
            if (!found.Contains(key))
            {
                notFound.Add(key);
            }
        }

        foreach (var remove in notFound)
        {
            System.Console.WriteLine("Removing device.");
            _devices.Remove(remove);
        }
    }
}