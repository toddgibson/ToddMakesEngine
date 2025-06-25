using System.Numerics;
using Raylib_cs;

namespace Engine.Components2D;

public enum CollisionShape2D
{
    None,
    Rectangle,
    Circle,
    Line
}

public enum SpriteMode
{
    Single,
    Framed
}

public class Sprite : Component2D
{
    public Sprite()
    {
        DrawLayer = 1;
    }
    
    private Rectangle _drawRect;
    private int _currentFrame = 0;
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; } = Color.White;
    public SpriteMode Mode { get; set; } = SpriteMode.Single;
    public Vector2 FrameSize { get; set; } = Vector2.NaN;

    public int CurrentFrame
    {
        get => _currentFrame;
        set
        {
            if (value > FrameCount)
                _currentFrame = 0;
            if (value < 0)
                _currentFrame = FrameCount;
            _currentFrame = value;
        }
    }

    private float _frameWidth => Mode == SpriteMode.Single ? Texture.Width : FrameSize.X;
    private float _frameHeight => Mode == SpriteMode.Single ? Texture.Height : FrameSize.Y;
    
    private int FrameCount => Mode == SpriteMode.Single ? 1 : Texture.Width / (int)FrameSize.X;

    internal override void Draw()
    {
        _drawRect = new Rectangle(WorldRectangle.X + PivotPoint.X, WorldRectangle.Y + PivotPoint.Y, WorldRectangle.Width, WorldRectangle.Height);
        Raylib.DrawTexturePro(Texture, new Rectangle(CurrentFrame * _frameWidth, CurrentFrame * _frameHeight, _frameWidth, _frameHeight), _drawRect, PivotPoint, LocalRotation, Tint);
    }
}