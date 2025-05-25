using System.Data;
using Engine.Logging;
using ZLinq;

namespace Engine;

public class SceneManager
{
    public static Action<string, int>? SceneChanged { get; set; }
    
    private readonly List<Scene> _scenes = [];
    public Scene? CurrentScene => NavigationStack.FirstOrDefault();
    
    private Stack<Scene> NavigationStack = new();
    
    public void SetActiveScene(Scene scene, bool clearNavigationStack = true)
    {
        if (CurrentScene == scene) return;
        
        CurrentScene?.Deactivated();
        
        if (clearNavigationStack)
            NavigationStack.Clear();
        
        NavigationStack.Push(scene);
        SceneChanged?.Invoke(scene.Name, NavigationStack.Count);
        scene.Activated();
    }
    public void SetActiveScene(string sceneName, bool clearNavigationStack = true) => SetActiveScene(GetSceneByName(sceneName) ?? throw new InvalidOperationException($"Scene with name {sceneName} not found."), clearNavigationStack);
    
    public void NavigateToScene(Scene scene) => SetActiveScene(scene, false);
    public void NavigateToScene(string sceneName) => SetActiveScene(sceneName, false);
    public void NavigateBack()
    {
        if (NavigationStack.Count == 1) return;
        CurrentScene?.Deactivated();
        NavigationStack.Pop();
        SceneChanged?.Invoke(CurrentScene?.Name ?? throw new ConstraintException("There is no active scene."), NavigationStack.Count);
        CurrentScene?.Activated();
    }

    public Scene? GetSceneByName(string name) => _scenes.AsValueEnumerable().FirstOrDefault(p => p.Name == name);

    internal void AddScene(Scene scene)
    {
        if (GetSceneByName(scene.Name) is not null)
            throw new ArgumentException($"Scene with name '{scene.Name}' already exists.");
        scene.Initialize();
        Log.Info($"Scene '{scene.Name}' initialized.");
        _scenes.Add(scene);
    }

    internal void RemoveScene(Scene scene) => _scenes.Remove(scene);
}