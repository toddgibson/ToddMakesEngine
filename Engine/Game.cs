using Engine.Systems;
using Raylib_cs;

namespace Engine;

public class Game : IDisposable
{
    public readonly GameOptions GameOptions;

    protected Game(GameOptions gameOptions)
    {
        GameOptions = gameOptions;
    }

    public virtual void Initialize()
    {
        UiSystem.Initialize();
    }

    public void Update()
    {
        var deltaTime = Raylib.GetFrameTime();
        TweenSystem.Update(deltaTime);
        UiSystem.Update(deltaTime);
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(GameOptions.ClearColor);
        Raylib.DrawFPS(12, 12);
        
        // DRAW GAME COMPONENTS
        //Raylib.DrawText("Hello, world!", 12, 42, 20, Color.White);

        // END DRAW GAME COMPONENTS
        
        // DRAW UI
        UiSystem.Draw();
        // END DRAW UI
        
        Raylib.EndDrawing();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        UiSystem.Cleanup();
    }

    ~Game()
    {
        Dispose(false);
    }
}

public class GameOptions
{
    public Color ClearColor { get; set; } = Color.Black;
    public int TargetFrameRate { get; set; }
    public int ScreenWidth { get; set; } = 1280;
    public int ScreenHeight { get; set; } = 720;
    public string GameTitle { get; set; } = "Todd Makes Game";
}