using HidSharp;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command;
using Opticall.Console.Command.Commands;

namespace Opticall.Console.Luxafor;

public class LuxaforDeviceManager : ILuxaforDeviceManager
{
    private readonly ILogger<LuxaforDeviceManager> _logger;
    private readonly IDictionary<string, LuxaforDevice> _devices = new Dictionary<string, LuxaforDevice>();
    private readonly DeviceList _deviceList;

    public LuxaforDeviceManager(ILogger<LuxaforDeviceManager> logger)
    {
        _logger = logger;
        _deviceList = DeviceList.Local;
        _deviceList.Changed += DeviceListChanged;
    }

    public void Initialise()
    {
        RefreshDevices();
    }

    public void Run(ICommand command)
    {
        try
        {
            foreach (var device in _devices)
            {
                _logger.LogDebug("Writing command to device.");
                device.Value.Run(command);
            }
        }
        catch (FileNotFoundException nfne)
        {
            _logger.LogInformation("Failed to call a device. It may be the list is stale so need to refresh the devices.");
            RefreshDevices();
            Run(command);
        }
    }

    private void DeviceListChanged(object? sender, DeviceListChangedEventArgs e)
    {
        Thread.Sleep(TimeSpan.FromSeconds(5));
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

            var luxaforDevice = new LuxaforDevice(hid, _logger);
            _devices.Add(hid.DevicePath, luxaforDevice);
            _logger.LogInformation("Adding Luxafor device.");
            _logger.LogDebug($"Device path: {hid.DevicePath}");
            found.Add(hid.DevicePath);

            var pattern = new PatternCommand
            {
                Pattern = PatternType.Rainbow,
                Repeat = 2
            };

            luxaforDevice.Run(pattern);
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
            _logger.LogInformation("Removing Luxafor device.");
            _logger.LogDebug($"Device path: {remove}");
            _devices.Remove(remove);
        }
    }
}