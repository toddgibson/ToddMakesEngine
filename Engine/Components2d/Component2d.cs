using System.Numerics;
using Raylib_cs;

namespace Engine.Components2d;

public class Component2d
{
    protected Entity Entity { get; private set; }
    
    public Vector2 LocalPosition { get; set; }
    public Vector2 GlobalPosition => Entity.Position + LocalPosition;
    public uint DrawLayer { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public float LocalRotation { get; set; }
    public float GlobalRotation => Entity.Rotation + LocalRotation;
    public Vector2 PivotPoint { get; set; }
    public Vector2 Size { get; set; }
    public bool Active { get; set; } = true;
    
    public CollisionShape2D CollisionShape { get; set; } = CollisionShape2D.None;

    internal virtual void Update(float deltaTime) { }
    internal virtual void Draw() { }
    internal void SetEntity(Entity entity) => Entity = entity;
    
    public Rectangle WorldRectangle
    {
        get
        {
            var actualPosition = GlobalPosition - PivotPoint;
            return new Rectangle(actualPosition.X, actualPosition.Y, Size.X * Scale.X, Size.Y * Scale.Y);
        }
    }

    public bool IsHovered
    {
        get
        {
            var mousePosition = Raylib.GetMousePosition();
            return mousePosition.X >= WorldRectangle.X
                   && mousePosition.X <= WorldRectangle.X + WorldRectangle.Width
                   && mousePosition.Y >= WorldRectangle.Y
                   && mousePosition.Y <= WorldRectangle.Y + WorldRectangle.Height;
        }
    }
}