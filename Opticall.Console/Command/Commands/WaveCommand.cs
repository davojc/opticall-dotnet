using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Command.Commands;

public class WaveCommand : ICommand
{
    [JsonPropertyName("wave")]
    public WaveType Wave { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("speed")]
    public byte Speed { get; set; } = 10;

    [JsonPropertyName("repeat")]
    public byte Repeat { get; set; } = 10;

    public IEnumerable<byte> ToBytes()
    {
        var rgb = ColorConverter.HexToRgb(Color);

        yield return 0;
        yield return (byte)CommandType.Wave;
        yield return (byte)Wave;
        yield return rgb.r;
        yield return rgb.g;
        yield return rgb.b;
        yield return Repeat;
        yield return Speed;
        yield return 0;
    }
}