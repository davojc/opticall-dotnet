using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Command.Commands;

public class FadeCommand : ICommand
{
    [JsonPropertyName("led")]
    public Led Led { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("time")]
    public byte Time { get; set; } = 10;

    public IEnumerable<byte> ToBytes()
    {
        var rgb = ColorConverter.HexToRgb(Color);

        yield return 0;
        yield return (byte)CommandType.Fade;
        yield return (byte)Led;
        yield return rgb.r;
        yield return rgb.g;
        yield return rgb.b;
        yield return Time;
        yield return 0;
        yield return 0;
    }
}