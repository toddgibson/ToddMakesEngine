using Raylib_cs;

namespace Engine;

public class GameSettings
{
    public Color ClearColor { get; set; } = Color.Black;
    public int TargetFrameRate { get; set; }
    public int ScreenWidth { get; set; } = 1280;
    public int ScreenHeight { get; set; } = 720;
    public string GameTitle { get; set; } = "Todd Makes Game";
}