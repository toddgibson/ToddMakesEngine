using Raylib_cs;

namespace Engine.Logging;

public static class Log
{
    public static void Info(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[{Raylib.GetTime():#.000}] {msg}");
        Console.ResetColor();
    }
    
    public static void Warning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"[{Raylib.GetTime():#.000}] {msg}");
        Console.ResetColor();
    }
    
    public static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"[{Raylib.GetTime():#.000}] {msg}");
        Console.ResetColor();
    }
    
    public static void Error(Exception e) => Error(e.ToString());
}