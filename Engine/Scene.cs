using Engine.Ui.Components;
using Raylib_cs;
using ZLinq;

namespace Engine;

public abstract class Scene(Game game, string name)
{
    public readonly Guid Id = Guid.NewGuid(); 
    public string Name { get; } = name;
    protected Game Game { get; } = game;
    
    internal readonly List<UiComponent> UiComponents = [];
    internal readonly List<IntervalAction> IntervalActions = [];
    internal readonly List<Entity> Entities = [];
    
    protected T AddUiComponent<T>(T component) where T : UiComponent
    {
        UiComponents.Add(component);
        return component;
    }

    protected void RemoveUiComponent<T>(T component) where T : UiComponent => UiComponents.Remove(component);
    
    protected T AddEntity<T>(T entity) where T : Entity
    {
        Entities.Add(entity);
        return entity;
    }

    protected void RemoveEntity<T>(T entity) where T : Entity => Entities.Remove(entity);

    protected List<T> GetEntitiesOfType<T>() where T : Entity =>
        Entities.AsValueEnumerable().Where(p => p is T && p.Active)
            .Cast<T>()
            .ToList(); 
    
    protected internal abstract void Initialize();
    protected internal virtual void Activated() { }
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
        foreach (var entity in Entities.AsValueEnumerable().Where(p => p.Active))
        {
            entity.DrawInternal();
        }
    }
    
    protected internal virtual void Update(float deltaTime) { }

    protected void RunAtInterval(Action action, float delaySeconds)
    {
        IntervalActions.Add(new IntervalAction(action, delaySeconds));
    }
    
    protected void CancelInterval(Action action)
    {
        var intervalAction = IntervalActions.FirstOrDefault(p => p.Action == action);
        if (intervalAction == null) return;
        IntervalActions.Remove(intervalAction);
    }
    
    protected void CancelAllIntervals() => IntervalActions.Clear();
}

internal class IntervalAction(Action action, float delaySeconds)
{
    internal Action Action { get; } = action;
    private double DelaySeconds { get; } = delaySeconds;
    private double LastRunTime { get; set; }
    
    internal bool ShouldRun => Raylib.GetTime() > LastRunTime + DelaySeconds;
    internal void Run()
    {
        LastRunTime = Raylib.GetTime();
        Action();
    }
}