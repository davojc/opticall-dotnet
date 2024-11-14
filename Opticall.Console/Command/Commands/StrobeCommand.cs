using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Command.Commands;

public class StrobeCommand : ICommand
{
    [JsonPropertyName("led")]
    public Led Led { get; set; }

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
        yield return (byte)CommandType.Strobe;
        yield return (byte)Led;
        yield return rgb.r;
        yield return rgb.g;
        yield return rgb.b;
        yield return Speed;
        yield return 0;
        yield return Repeat;
    }
}