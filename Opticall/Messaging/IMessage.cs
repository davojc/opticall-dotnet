using Opticall.Luxafor.Commands;

namespace Opticall.Messaging;

public interface IMessage
{
    RequestType Request { get; }

    string Target { get; }

    string Pattern { get; }

    byte? Speed { get; }

    byte? Repeat { get; }

    byte? Red { get; }

    byte? Green { get; }

    byte? Blue { get; }
}