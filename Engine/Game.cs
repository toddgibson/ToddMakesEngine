using System.Data;
using Engine.Systems;
using Raylib_cs;
using ZLinq;

namespace Engine;

public abstract class Game(GameSettings settings) : IDisposable
{
    public readonly GameSettings Settings = settings;
    public readonly SceneManager SceneManager = new();

    internal void InitializeInternal()
    {
        UiSystem.Initialize();
        Initialize();
    }

    protected virtual void Initialize() { }

    protected void AddScene(Scene scene, bool makeActiveScene = false)
    {
        SceneManager.AddScene(scene);
        if (makeActiveScene)
            SetActiveScene(scene);
    }

    public void SetActiveScene(Scene sampleScene) => SceneManager.SetActiveScene(sampleScene);
    public void SetActiveScene(string sceneName) => SceneManager.SetActiveScene(sceneName);
    protected void RemoveScene(Scene scene) => SceneManager.RemoveScene(scene);

    internal void UpdateInternal()
    {
        if (SceneManager.CurrentScene is null)
            throw new ConstraintException("There is no active scene.");
        
        var deltaTime = Raylib.GetFrameTime();
        SceneManager.CurrentScene.Update(deltaTime);
        if (SceneManager.CurrentScene.IntervalActions.Count > 0)
        {
            foreach (var action in SceneManager.CurrentScene.IntervalActions
                         .AsValueEnumerable()
                         .Where(p => p.ShouldRun))
            {
                action.Run();
            }
        }
        TweenSystem.Update(deltaTime);
        UiSystem.Update(deltaTime, SceneManager.CurrentScene);
    }

    internal void DrawInternal()
    {
        if (SceneManager.CurrentScene is null)
            throw new ConstraintException("There is no active scene.");
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Settings.ClearColor);
        Raylib.DrawFPS(12, 12);
        
        // DRAW GAME COMPONENTS
        //Raylib.DrawText("Hello, world!", 12, 42, 20, Color.White);

        // END DRAW GAME COMPONENTS
        
        // DRAW UI
        UiSystem.Draw(SceneManager.CurrentScene);
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