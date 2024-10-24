using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opticall.Console.OSC;

namespace Opticall.Console;

public static class ToolExtensions
{
    public static string? ReadFirstArgAsString(this OscMessage message)
    {
        if (message.Arguments == null)
        {
            return null;
        }

        var args = message.Arguments.ToArray();

        if (args.Length == 0)
        {
            return null;
        }

        if (args[0] is string)
        {
            return args[0].ToString();
        }

        return null;
    }
}