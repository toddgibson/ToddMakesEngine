using Engine.Components2d;
using Engine.Entities;
using Engine.Ui.Components;
using Raylib_cs;
using ZLinq;

namespace Engine;

public abstract class Scene(Game game, string name)
{
    public readonly Guid Id = Guid.NewGuid(); 
    public string Name { get; } = name;
    public Game Game { get; internal set; } = game;
    
    internal readonly List<UiComponent> UiComponents = [];
    internal readonly List<IntervalAction> IntervalActions = [];
    internal readonly List<Entity> Entities = [];

    
    protected T AddUiComponent<T>(T component) where T : UiComponent
    {
        UiComponents.Add(component);
        return component;
    }

    protected UiComponent? GetUiComponentByName(string name) => UiComponents.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    protected void RemoveUiComponent<T>(T component) where T : UiComponent => UiComponents.Remove(component);
    
    protected T AddEntity<T>(T entity) where T : Entity
    {
        Entities.Add(entity);
        entity.Scene = this;
        entity.Initialize();
        return entity;
    }

    protected void RemoveEntity<T>(T entity) where T : Entity => Entities.Remove(entity);

    public T? GetFirstEntityOfType<T>() where T : Entity =>
        GetEntitiesOfType<T>().FirstOrDefault();
    
    public List<T> GetEntitiesOfType<T>() where T : Entity =>
        Entities.AsValueEnumerable().Where(p => p is T && p.Active)
            .Cast<T>()
            .ToList(); 
    
    protected internal abstract void Initialize();
    protected internal virtual void Activated(object? payload = null) { }
    protected internal virtual void Deactivated() { }

    internal void UpdateInternal(float deltaTime)
    {
        foreach (var entity in Entities.AsValueEnumerable().Where(p => p.Active))
        {
            entity.UpdateInternal(deltaTime);
            entity.Update(deltaTime);
        }
    }
    
    internal void DrawInternal()
    {
        var sortedComponents = Entities
            .AsValueEnumerable()
            .Where(p => p.Active)
            .SelectMany(p => p.Component2Ds)
            .Where(p => p.Active 
                        && p.IsWithinVisibleBounds(Game.CameraBounds))
            .OrderBy(p => p.DrawLayer)
            .ThenBy(p => p.GlobalPosition.Y);
        
        foreach (var component2d in sortedComponents)
            component2d.Draw();
    }
    
    protected internal virtual void Update(float deltaTime) { }

    protected void RunAtInterval(Action action, float delaySeconds, bool singleRunOnly = false)
    {
        IntervalActions.Add(new IntervalAction(action, delaySeconds, singleRunOnly));
    }
    
    protected internal void CancelInterval(Action action)
    {
        var intervalAction = IntervalActions.FirstOrDefault(p => p.Action == action);
        if (intervalAction == null) return;
        IntervalActions.Remove(intervalAction);
    }
    
    protected void CancelAllIntervals() => IntervalActions.Clear();
}

internal class IntervalAction(Action action, float delaySeconds, bool singleRunOnly = false)
{
    internal Action Action { get; } = action;
    private double DelaySeconds { get; } = delaySeconds;
    private double LastRunTime { get; set; } = singleRunOnly ? Raylib.GetTime() : 0;
    internal bool SingleRunOnly { get; } = singleRunOnly;
    
    internal bool ShouldRun => Raylib.GetTime() > LastRunTime + DelaySeconds;
    internal void Run()
    {
        LastRunTime = Raylib.GetTime();
        Action();
    }
}