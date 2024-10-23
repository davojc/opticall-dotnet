using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opticall.Console.OSC;

public class OscMessage(Address address, IEnumerable<object>? arguments = null)
{
    public Address Address { get; } = address;
    public IEnumerable<object>? Arguments { get; } = arguments;
}