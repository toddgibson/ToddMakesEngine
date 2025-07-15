using Raylib_cs;

namespace Engine.Extensions;

public static class ColorHelpers
{
    public static Color GetRandomColor()
    {
        var r = (byte)Raylib.GetRandomValue(0, 255);
        var g = (byte)Raylib.GetRandomValue(0, 255);
        var b = (byte)Raylib.GetRandomValue(0, 255);
        return new Color(r, g, b, (byte)255);
    }

    public static Color Add(this Color color, byte value)
    {
        return color.Add(value, value, value);
    }
    
    public static Color Add(this Color color, byte r, byte g, byte b)
    {
        return new Color((byte)Raymath.Clamp(color.R + r, 0, 255), (byte)Raymath.Clamp(color.G + g, 0, 255), (byte)Raymath.Clamp(color.B + b, 0, 255));
    }
}