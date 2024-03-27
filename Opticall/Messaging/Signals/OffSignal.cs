using System.Text.Json.Serialization;
using Opticall.Luxafor;

namespace Opticall.Messaging.Signals;

[Signal(SignalType.Off)]
[CommandTemplate((byte)CommandType.Color, (byte)Led.All, 0, 0, 0, 0, 0, 0)]
public record OffSignal : ISignalTopic
{
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    [CommandField(1)]
    [JsonPropertyName("led")]
    public Led Led { get; set; }
}
