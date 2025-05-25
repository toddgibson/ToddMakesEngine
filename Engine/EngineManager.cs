using Engine.Logging;

namespace Engine;
using Raylib_cs ;

public static class EngineManager
{
    public static void Run(Game game)
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(game.Settings.ScreenWidth, game.Settings.ScreenHeight, game.Settings.GameTitle);
        if (game.Settings.TargetFrameRate > 0)
            Raylib.SetTargetFPS(game.Settings.TargetFrameRate);
        
        try
        {
            game.InitializeInternal();
            
            while (!Raylib.WindowShouldClose())
            {
                game.UpdateInternal();
                game.DrawInternal();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }

        game.Dispose();

        Raylib.CloseWindow();
    }
}