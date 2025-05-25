using Engine.Ui.Components;
using Raylib_cs;

namespace Engine;

public abstract class Scene(Game game, string name)
{
    public readonly Guid Id = Guid.NewGuid(); 
    public string Name { get; } = name;
    protected Game Game { get; } = game;
    
    internal List<UiComponent> UiComponents = [];
    internal List<IntervalAction> IntervalActions = [];
    
    protected void AddUiComponent(UiComponent component) => UiComponents.Add(component);
    protected void RemoveUiComponent(UiComponent component) => UiComponents.Remove(component);
    
    protected internal abstract void Initialize();
    protected internal virtual void Activated() { }
    protected internal virtual void Deactivated() { }
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