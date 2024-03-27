using System.Text.Json.Serialization;
using Opticall.Luxafor;

namespace Opticall.Messaging.Signals;

[Signal(SignalType.Pattern)]
[CommandTemplate((byte)CommandType.Pattern, 0, 0)]
public record PatternSignal : ISignalTopic
{
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    [JsonPropertyName("type")]
    [CommandField(1)]
    public PatternType? Type { get; set; }

    [JsonPropertyName("repeat")]
    [CommandField(2)]
    public byte? Repeat { get; set; }
}
