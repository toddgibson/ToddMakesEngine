using Engine.Logging;
using Raylib_cs;
using Engine.Ui.Components;
using ZLinq;

namespace Engine.Systems;

public static class UiSystem
{
    private static readonly HashSet<UiComponent> UiComponents = [];
    public static Font DefaultFont { get; private set; }
    public static Texture2D DefaultButtonTexture { get; private set; }

    internal static void Initialize()
    {
        try
        {
            DefaultFont = Raylib.LoadFontEx("DefaultEngineAssets/Brawler-Bold.ttf", 96, null, 0);
            DefaultButtonTexture = Raylib.LoadTexture("DefaultEngineAssets/basic-button.png");
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
    
    internal static void AddComponent(UiComponent component) => UiComponents.Add(component);
    internal static void RemoveComponent(UiComponent component) => UiComponents.Remove(component);

    internal static void Update(float delta)
    {
        foreach (var component in UiComponents.AsValueEnumerable().Where(p => p.Active))
            component.Update(delta);
    }

    internal static void Draw()
    {
        foreach (var component in UiComponents.AsValueEnumerable().Where(p => p.Active))
            component.Draw();
    }

    internal static void Cleanup()
    {
        Raylib.UnloadFont(DefaultFont);
        Raylib.UnloadTexture(DefaultButtonTexture);
    }
}