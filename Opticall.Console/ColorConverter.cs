using System.Text.RegularExpressions;

namespace Opticall.Console;

public static class ColorConverter
{
    private static Regex _htmlColorRegex = new Regex("^#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{6})$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    // Method to convert RGB to Hex
    public static string RgbToHex(byte r, byte g, byte b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    // Method to convert Hex to RGB
    public static (byte r, byte g, byte b) HexToRgb(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
        {
            throw new ArgumentException("Cannot find the Hex 'color' parameter.");
        }

        if (!_htmlColorRegex.IsMatch(hex))
        {
            throw new ArgumentException("Hex 'color' parameter does not seem to follow the standard '#000000' format.");
        }

        hex = hex.TrimStart('#');
        var r = Convert.ToByte(hex.Substring(0, 2), 16);
        var g = Convert.ToByte(hex.Substring(2, 2), 16);
        var b = Convert.ToByte(hex.Substring(4, 2), 16);
        return (r, g, b);
    }
}