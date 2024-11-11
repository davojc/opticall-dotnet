using System.Text.Json.Serialization;
using Opticall.Console.Commands;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Model;

public record On
{
    [JsonPropertyName("color")]
    public string Color { get; set; }
}

public record Off
{
}


public static class ModelExtensions
{
    public static byte[] ToBytes(this On on, Led led)
    {
        var rgb = ColorConverter.HexToRgb(on.Color);

        var input = Enumerable.Repeat((byte)0, 4).ToArray();

        input[0] = (byte)led;
        input[1] = (byte)rgb.r;
        input[2] = (byte)rgb.g;
        input[3] = (byte)rgb.b;

        return input;
    }

    public static byte[] ToBytes(this Off on, Led led)
    {
        var input = Enumerable.Repeat((byte)0, 1).ToArray();

        input[0] = (byte)led;

        return input;
    }
}
