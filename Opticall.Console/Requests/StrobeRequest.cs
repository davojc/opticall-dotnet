using System.Text.Json.Serialization;

namespace Opticall.Console.Requests;

public record StrobeRequest : IRequest
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("speed")]
    public byte Speed { get; set; } = 10;

    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; } = 10;

    public byte[] ToBytes()
    {
        var input = Enumerable.Repeat((byte)0, 5).ToArray();
        var rgb = ColorConverter.HexToRgb(Color);

        input[0] = (byte)rgb.r;
        input[1] = (byte)rgb.g;
        input[2] = (byte)rgb.b;
        input[3] = (byte)Speed;
        input[4] = (byte)Repeat;

        return input;
    }
}