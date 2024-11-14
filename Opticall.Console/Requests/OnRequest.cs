using System.Text.Json.Serialization;

namespace Opticall.Console.Requests;

public record OnRequest : IRequest
{
    [JsonPropertyName("color")]
    public required string Color { get; set; }

    public byte[] ToBytes()
    {
        var rgb = ColorConverter.HexToRgb(Color);

        var input = Enumerable.Repeat((byte)0, 3).ToArray();

        input[0] = (byte)rgb.r;
        input[1] = (byte)rgb.g;
        input[2] = (byte)rgb.b;

        return input;
    }
}