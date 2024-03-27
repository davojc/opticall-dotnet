using System.Text.Json.Serialization;
using Opticall.Luxafor;

namespace Opticall.Messaging.Signals;

[Signal(SignalType.Wave)]
[CommandTemplate((byte)CommandType.Wave, (byte)Led.All, 0, 0, 0, 0, 0, 0)]
public record WaveSignal : ISignalTopic
{
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    [CommandField(1)]
    [JsonPropertyName("led")]
    public Led Led { get; set; }
    
    [CommandField(1)]
    [JsonPropertyName("type")]
    public WaveType Type { get; set; }

    [CommandField(2)]
    [JsonPropertyName("red")]
    public byte Red { get; set; }

    [CommandField(3)]
    [JsonPropertyName("green")]
    public byte Green { get; set; }

    [CommandField(4)]
    [JsonPropertyName("blue")]
    public byte Blue { get; set; }

    [CommandField(7)]
    [JsonPropertyName("speed")]
    public byte Speed { get; set; }

    [CommandField(6)]
    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; }
}
