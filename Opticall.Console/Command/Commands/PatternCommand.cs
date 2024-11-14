using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Command.Commands;

public class PatternCommand : ICommand
{
    [JsonPropertyName("pattern")] 
    public required PatternType Pattern { get; set; } = PatternType.TrafficLight;

    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; } = 3;

    public IEnumerable<byte> ToBytes()
    {
        yield return 0;
        yield return (byte)CommandType.Pattern;
        yield return (byte)Pattern;
        yield return Repeat;
    }
}