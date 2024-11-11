namespace Opticall.Console;

public static class ColorConverter
{
    // Method to convert RGB to Hex
    public static string RgbToHex(int r, int g, int b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    // Method to convert Hex to RGB
    public static (int r, int g, int b) HexToRgb(string hex)
    {
        hex = hex.TrimStart('#');
        int r = Convert.ToInt32(hex.Substring(0, 2), 16);
        int g = Convert.ToInt32(hex.Substring(2, 2), 16);
        int b = Convert.ToInt32(hex.Substring(4, 2), 16);
        return (r, g, b);
    }
}