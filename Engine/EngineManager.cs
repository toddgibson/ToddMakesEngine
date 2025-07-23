using Engine.Logging;

namespace Engine;
using Raylib_cs ;

public static class EngineManager
{
    internal static Music? CurrentMusic;
    
    public static void Run(Game game)
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(game.Settings.ScreenWidth, game.Settings.ScreenHeight, game.Settings.GameTitle);
        if (game.Settings.TargetFrameRate > 0)
            Raylib.SetTargetFPS(game.Settings.TargetFrameRate);
        
        Raylib.InitAudioDevice();
        Raylib.SetMasterVolume(1f);
        
        try
        {
            game.InitializeInternal();
            
            while (!game.ShouldQuit)
            {
                game.UpdateInternal();
                game.DrawInternal();
                
                if (CurrentMusic.HasValue)
                    Raylib.UpdateMusicStream(CurrentMusic.Value);
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