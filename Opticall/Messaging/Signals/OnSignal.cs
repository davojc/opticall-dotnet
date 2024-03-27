using System.Text.Json.Serialization;
using Opticall.Luxafor;

namespace Opticall.Messaging.Signals;

[Signal(SignalType.On)]
[CommandTemplate((byte)CommandType.Color, (byte)Led.All, 0, 0, 0, 0, 0, 0)]
public record OnSignal : ISignalTopic
{
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    [CommandField(1)]
    [JsonPropertyName("led")]
    public Led Led { get; set; }

    [CommandField(2)]
    [JsonPropertyName("red")]
    public byte Red { get; set; }

    [CommandField(3)]
    [JsonPropertyName("green")]
    public byte Green { get; set; }

    [CommandField(4)]
    [JsonPropertyName("blue")]
    public byte Blue { get; set; }
}
