using System.Numerics;
using Engine.Entities;
using Raylib_cs;

namespace Engine.Components2d;

public class Component2d
{
    protected Entity Entity { get; private set; }
    
    public Vector2 LocalPosition { get; set; }
    public Vector2 GlobalPosition => Entity.Position + LocalPosition;
    public uint DrawLayer { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public float LocalRotation { get; private set; }
    public float GlobalRotation => Entity.Rotation + LocalRotation;
    public Vector2 PivotPoint { get; set; }
    public Vector2 Size { get; set; }
    public bool Active { get; set; } = true;
    
    public CollisionShape2D CollisionShape { get; set; } = CollisionShape2D.None;

    internal virtual void Initialize() { }
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
    
    private readonly HashSet<Component2d> _collidingWithLastFrame = new();

    public bool IsCollidingWith(Component2d other)
    {
        return Raylib.CheckCollisionRecs(WorldRectangle, other.WorldRectangle);
    }
    
    public bool CollisionJustOccuredWith(Component2d other)
    {
        bool isColliding = Raylib.CheckCollisionRecs(WorldRectangle, other.WorldRectangle);
        bool wasColliding = _collidingWithLastFrame.Contains(other);

        if (isColliding && !wasColliding)
        {
            _collidingWithLastFrame.Add(other);
            return true; // collision just started
        }

        if (!isColliding && wasColliding)
        {
            _collidingWithLastFrame.Remove(other); // collision ended
        }

        return false;
    }
    
    // internal Vector2[] GetRotatedCorners()
    // {
    //     float rot = MathF.PI * LocalRotation / 180f;
    //     float cos = MathF.Cos(rot);
    //     float sin = MathF.Sin(rot);
    //
    //     // Define local corners relative to origin
    //     Vector2 topLeft     = -PivotPoint;
    //     Vector2 topRight    = new Vector2(Size.X - PivotPoint.X, -PivotPoint.Y);
    //     Vector2 bottomRight = new Vector2(Size.X - PivotPoint.X, Size.Y - PivotPoint.Y);
    //     Vector2 bottomLeft  = new Vector2(-PivotPoint.X, Size.Y - PivotPoint.Y);
    //
    //     // Rotate and translate
    //     Vector2 Rotate(Vector2 v) => new Vector2(
    //         v.X * cos - v.Y * sin,
    //         v.X * sin + v.Y * cos
    //     );
    //     
    //     return new[]
    //     {
    //         LocalPosition + Rotate(topLeft),
    //         LocalPosition + Rotate(topRight),
    //         LocalPosition + Rotate(bottomRight),
    //         LocalPosition + Rotate(bottomLeft)
    //     };
    // }
}