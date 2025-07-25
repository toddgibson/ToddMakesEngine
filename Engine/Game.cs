using System.Data;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using Engine.Systems;
using Raylib_cs;
using ZLinq;

namespace Engine;

public abstract class Game(GameSettings settings) : IDisposable
{
    public readonly GameSettings Settings = settings;
    public readonly SceneManager SceneManager = new();
    public Camera2D MainCamera = new(Vector2.Zero, Vector2.Zero, 0f, 0f);
    public Rectangle CameraBounds => new(
        Raylib.GetScreenToWorld2D(Vector2.Zero, MainCamera), // top-left world corner
        new Vector2(
            Raylib.GetScreenWidth() / MainCamera.Zoom,
            Raylib.GetScreenHeight() / MainCamera.Zoom
        )
    );

    internal bool ShouldQuit = false;
    public bool DisplayFrameData = false;

#if DEBUG
    private readonly List<double> _frameDrawTimes = [];
    private double AverageFrameDrawTime => _frameDrawTimes.Average();
    private readonly List<double> _frameUpdateTimes = [];
    private double AverageFrameUpdateTime => _frameUpdateTimes.Average();
    private double AverageFrameTime => AverageFrameDrawTime + AverageFrameUpdateTime;
#endif

    internal void InitializeInternal()
    {
        UiSystem.Initialize();
        Initialize();
        
        MainCamera.Target = new Vector2(Settings.ScreenWidth * 0.5f, Settings.ScreenHeight * 0.5f);
        MainCamera.Offset = new Vector2(Settings.ScreenWidth * 0.5f, Settings.ScreenHeight * 0.5f);
        MainCamera.Zoom = 1f;
    }

    protected virtual void Initialize() { }

    protected void AddScene(Scene scene, bool makeActiveScene = false)
    {
        SceneManager.AddScene(scene);
        scene.Game = this;
        if (makeActiveScene)
            SetActiveScene(scene);
    }

    public void SetActiveScene(Scene sampleScene, object? payload = null) => SceneManager.SetActiveScene(sampleScene, payload);
    public void SetActiveScene(string sceneName, object? payload = null) => SceneManager.SetActiveScene(sceneName, payload);
    protected void RemoveScene(Scene scene) => SceneManager.RemoveScene(scene);

    internal void UpdateInternal()
    {
        ShouldQuit = Raylib.WindowShouldClose() && !Raylib.IsKeyPressed(KeyboardKey.Escape);
        
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
                if (action.SingleRunOnly)
                    SceneManager.CurrentScene.CancelInterval(action.Action);
            }
        }
        TweenSystem.Update(deltaTime);
        UiSystem.Update(deltaTime, SceneManager.CurrentScene);
        
        SceneManager.CurrentScene.UpdateInternal(deltaTime);
    }

#if DEBUG
    private readonly Stopwatch _frameTimer = new();
#endif
    
    internal void DrawInternal()
    {
        if (SceneManager.CurrentScene is null)
            throw new ConstraintException("There is no active scene.");
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Settings.ClearColor);
        
        // DRAW GAME COMPONENTS
        Raylib.BeginMode2D(MainCamera);
#if DEBUG
        _frameTimer.Restart();
#endif
        
        SceneManager.CurrentScene.DrawInternal();
        
#if DEBUG
        _frameTimer.Stop();
        _frameDrawTimes.Add(_frameTimer.Elapsed.TotalMicroseconds / 1000);
        if (_frameDrawTimes.Count > 100)
            _frameDrawTimes.RemoveAt(0);
#endif
        Raylib.EndMode2D();
        // END DRAW GAME COMPONENTS
        
        // DRAW UI
#if DEBUG
        _frameTimer.Restart();
#endif
        
        UiSystem.Draw(SceneManager.CurrentScene);
        
#if DEBUG
        _frameTimer.Stop();
        _frameUpdateTimes.Add(_frameTimer.Elapsed.TotalMicroseconds / 1000);
        if (_frameUpdateTimes.Count > 100)
            _frameUpdateTimes.RemoveAt(0);
#endif
        // END DRAW UI

        if (DisplayFrameData)
        {
            Raylib.DrawFPS(12, 12);
            
#if DEBUG
            Raylib.DrawText($"Avg. Frame Time: {AverageFrameTime:0.000}ms, Draw: {AverageFrameDrawTime:0.000}ms, Update: {AverageFrameUpdateTime:0.000}ms", 12, 32, Raylib.GetFontDefault().BaseSize, Color.DarkGreen);
            Raylib.DrawText($"Memory Usage: {Process.GetCurrentProcess().WorkingSet64 / (1024.0 * 1024.0):F2}mb", 12, 48, Raylib.GetFontDefault().BaseSize, Color.DarkGreen);
            Raylib.DrawText($"Heap Allocation: {GC.GetTotalMemory(false) / (1024.0 * 1024.0):F2}mb", 12, 64, Raylib.GetFontDefault().BaseSize, Color.DarkGreen);
#endif
        }
        
        Raylib.EndDrawing();
    }
    
    public void Quit() => ShouldQuit = true;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        UiSystem.Cleanup();
        AssetManager.UnloadAllAssets();
    }

    ~Game()
    {
        Dispose(false);
    }
}