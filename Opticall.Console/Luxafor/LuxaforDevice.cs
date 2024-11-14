using System.Runtime.InteropServices;
using HidSharp;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command;

namespace Opticall.Console.Luxafor;

public class LuxaforDevice(HidDevice hidDevice, ILogger logger) : ILuxaforDevice
{
    public string DeviceId => hidDevice.DevicePath;

    public void Run(ICommand command)
    {
        if(command == null)
            return;

        var cmd = command.ToBytes().ToArray();

        // For some reason array needs to be shifted by 1 for windows.
        /*
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Array.Resize(ref cmd, cmd.Length + 1);
            Array.Copy(cmd, 0, cmd, 1, cmd.Length -1 );
            cmd[0] = 0;
        }
        */

        if (hidDevice.TryOpen(out DeviceStream deviceStream))
        {
            deviceStream.Write(cmd, 0, cmd.Length);
            deviceStream.Close();
        }
        else
        {
            logger.LogWarning("Couldn't open device stream.");
        }
    }
}
