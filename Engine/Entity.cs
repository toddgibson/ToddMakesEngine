using System.Numerics;
using Engine.Components2D;
using ZLinq;

namespace Engine;

public class Entity(string name)
{
    public readonly Guid Id = Guid.NewGuid(); 
    public string Name { get; } = name;
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 PivotPoint { get; set; }
    public bool Active { get; set; } = true;
    
    internal readonly List<Component2D> Component2Ds = [];
    
    public Entity AddComponent2D<T>(T component) where T : Component2D
    {
        component.SetEntity(this);
        Component2Ds.Add(component);
        return this;
    }

    public void RemoveComponent2D(Component2D component) => Component2Ds.Remove(component);
    
    public T? GetComponentOfType<T>() where T : Component2D =>
        Component2Ds.AsValueEnumerable().Where(p => p is T && p.Active)
            .Cast<T>()
            .FirstOrDefault(); 
    
    public List<T> GetComponentsOfType<T>() where T : Component2D =>
        Component2Ds.AsValueEnumerable().Where(p => p is T && p.Active)
            .Cast<T>()
            .ToList(); 

    internal void UpdateInternal(float deltaTime)
    {
        foreach (var component in Component2Ds.AsValueEnumerable().Where(p => p.Active))
        {
            component.Update(deltaTime);
        }
    }
    
    public virtual void Update(float deltaTime) { }
    
    internal void DrawInternal()
    {
        foreach (var component in Component2Ds.AsValueEnumerable().Where(p => p.Active))
            component.Draw();
    }
}