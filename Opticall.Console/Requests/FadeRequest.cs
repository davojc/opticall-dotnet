using System.Text.Json.Serialization;

namespace Opticall.Console.Requests;

public record FadeRequest : IRequest
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("time")]
    public byte Time { get; set; } = 10;

    public byte[] ToBytes()
    {
        var input = Enumerable.Repeat((byte)0, 4).ToArray();
        var rgb = ColorConverter.HexToRgb(Color);

        input[0] = (byte)rgb.r;
        input[1] = (byte)rgb.g;
        input[2] = (byte)rgb.b;
        input[3] = (byte)Time;

        return input;
    }
}