using Opticall.Luxafor.Commands;

namespace Opticall.Messaging;

public struct Message : IMessage
{
    public RequestType Request { get; set; }

    public string Target { get; set; }

    public string Pattern { get; set; }

    public byte? Repeat { get; set; }

    public byte? Speed { get; set; }

    public byte? Red { get; set; }

    public byte? Green { get; set; }

    public byte? Blue { get; set; }
}


