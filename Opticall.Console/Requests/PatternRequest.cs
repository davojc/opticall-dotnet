using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Requests;

public record PatternRequest : IRequest
{
    [JsonPropertyName("pattern")]
    public PatternType Pattern { get; set; }

    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; } = 10;

    public byte[] ToBytes()
    {
        var input = Enumerable.Repeat((byte)0, 2).ToArray();

        input[0] = (byte)Pattern;
        input[1] = (byte)Repeat;

        return input;
    }
}