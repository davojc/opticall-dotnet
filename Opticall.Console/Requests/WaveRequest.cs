using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Requests;

public record WaveRequest : IRequest
{
    [JsonPropertyName("wave")]
    public WaveType Wave { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("speed")]
    public byte Speed { get; set; } = 10;

    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; } = 10;

    public byte[] ToBytes()
    {
        var input = Enumerable.Repeat((byte)0, 6).ToArray();
        var rgb = ColorConverter.HexToRgb(Color);

        input[0] = (byte)Wave;
        input[1] = (byte)rgb.r;
        input[2] = (byte)rgb.g;
        input[3] = (byte)rgb.b;
        input[4] = (byte)Speed;
        input[5] = (byte)Repeat;

        return input;
    }
}