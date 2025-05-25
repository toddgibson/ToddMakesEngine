using Raylib_cs;

namespace Engine;

public static class Random
{
    /// <summary>
    /// Get a random int value between min (inclusive) and max (inclusive)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Range(int min, int max) => Raylib.GetRandomValue(min, max);
    
    /// <summary>
    /// Get a random float value between min (inclusive) and max (inclusive)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Range(float min, float max)
    {
        const int precision = 100_000;
        var raw = Raylib.GetRandomValue(0, precision);
        var t = raw / (float)precision;
        return min + (max - min) * t;
    }
    
    /// <summary>
    /// Get a random byte value between 0 and max 255
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static byte Byte() => (byte)Raylib.GetRandomValue(0, 255);
}