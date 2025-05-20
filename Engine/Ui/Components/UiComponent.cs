using System.Numerics;
using Engine.Systems;
using Raylib_cs;
using Rectangle = Raylib_cs.Rectangle;

namespace Engine.Ui.Components;

public abstract class UiComponent
{
    protected UiComponent() => UiSystem.AddComponent(this);

    public bool Active { get; set; } = true;
    public Vector2 Size { get; set; } = new(64, 64);
    public Vector2 Position { get; set; }

    private Vector2 _scale = Vector2.One;
    public Vector2 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            if (Texture.HasValue)
                PivotPoint = new Vector2(Texture.Value.Width * Scale.X * 0.5f, Texture.Value.Height * Scale.Y * 0.5f);
        }
    }
    public float Rotation { get; set; }
    public Texture2D? Texture { get; set; }
    public Vector2 PivotPoint { get; set; } = Vector2.Zero;
    public Color TextureTint { get; set; } = Color.White;

    public enum AnchorEnum
    {
        None,
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }
    public AnchorEnum Anchor { get; set; } = AnchorEnum.None;
    public Vector2 AnchorPoint { get; private set; } = Vector2.Zero;
    
    public Rectangle ScreenRectangle
    {
        get
        {
            var screenWidth = Raylib.GetScreenWidth();
            var screenHeight = Raylib.GetScreenHeight();
            AnchorPoint = Anchor switch
            {
                AnchorEnum.TopLeft => new Vector2(0, 0),
                AnchorEnum.TopCenter => new Vector2(screenWidth * 0.5f, 0),
                AnchorEnum.TopRight => new Vector2(screenWidth, 0),
                AnchorEnum.MiddleLeft => new Vector2(0, screenHeight * 0.5f),
                AnchorEnum.MiddleCenter => new Vector2(screenWidth * 0.5f, screenHeight * 0.5f),
                AnchorEnum.MiddleRight => new Vector2(screenWidth, screenHeight * 0.5f),
                AnchorEnum.BottomLeft => new Vector2(0, screenHeight),
                AnchorEnum.BottomCenter => new Vector2(screenWidth * 0.5f, screenHeight),
                AnchorEnum.BottomRight => new Vector2(screenWidth, screenHeight),
                _ => new Vector2(0, 0)
            };
            var actualPosition = Position + AnchorPoint - PivotPoint;
            return new Rectangle(actualPosition.X, actualPosition.Y, Size.X * Scale.X, Size.Y * Scale.Y);
        }
    }

    public bool IsHovered
    {
        get
        {
            var mousePosition = Raylib.GetMousePosition();
            return mousePosition.X >= ScreenRectangle.X
                   && mousePosition.X <= ScreenRectangle.X + ScreenRectangle.Width
                   && mousePosition.Y >= ScreenRectangle.Y
                   && mousePosition.Y <= ScreenRectangle.Y + ScreenRectangle.Height;
        }
    }

    public abstract void Draw();

    public abstract void Update(float delta);
}