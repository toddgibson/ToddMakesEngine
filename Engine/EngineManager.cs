namespace Engine;
using Raylib_cs ;

public static class EngineManager
{
    public static void Run(Game game)
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(game.GameOptions.ScreenWidth, game.GameOptions.ScreenHeight, game.GameOptions.GameTitle);
        if (game.GameOptions.TargetFrameRate > 0)
            Raylib.SetTargetFPS(game.GameOptions.TargetFrameRate);

        game.Initialize();

        while (!Raylib.WindowShouldClose())
        {
            game.Update();
            game.Draw();
        }

        game.Dispose();

        Raylib.CloseWindow();
    }
}