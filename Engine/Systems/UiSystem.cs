using Engine.Logging;
using Raylib_cs;
using Engine.Ui.Components;
using ZLinq;

namespace Engine.Systems;

public static class UiSystem
{
    public static Font DefaultFont { get; private set; }
    public static Texture2D DefaultButtonTexture { get; private set; }
    public static bool IsCursorHoveringUi => HoveredUiComponent != null;
    public static UiComponent? HoveredUiComponent { get; private set; }

    internal static void Initialize()
    {
        try
        {
            DefaultFont = AssetManager.LoadFont("DefaultEngineAssets/Brawler-Bold.ttf", "engine_default_font", 96);
            DefaultButtonTexture = AssetManager.LoadTexture("DefaultEngineAssets/basic-button.png", "engine_default_button");
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        finally
        {
            Log.Info("UiSystem Initialized");
        }
    }

    internal static void Update(float delta, Scene scene)
    {
        HoveredUiComponent = null;
        foreach (var component in scene.UiComponents.AsValueEnumerable().Where(p => p.Active))
        {
            component.Update(delta);
            if (component.IsHovered)
                HoveredUiComponent = component;
        }
    }

    internal static void Draw(Scene scene)
    {
        foreach (var component in scene.UiComponents.AsValueEnumerable().Where(p => p.Active))
            component.Draw();
    }

    internal static void Cleanup()
    {
        Raylib.UnloadFont(DefaultFont);
        Raylib.UnloadTexture(DefaultButtonTexture);
    }
}