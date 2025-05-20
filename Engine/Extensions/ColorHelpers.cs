

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
}