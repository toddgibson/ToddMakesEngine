using System.Numerics;
using Raylib_cs;

namespace Engine.Components2d;

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

public class Sprite : Component2d
{
    private Shader _outlineShader;
    
    public Sprite()
    {
        DrawLayer = 1;
    }

    public Sprite(bool enableOutlineShader)
    {
        DrawLayer = 1;
        EnableOutlineShader = enableOutlineShader;
        
        if (EnableOutlineShader)
        {
            _outlineShader = Raylib.LoadShader(null, "Assets/Shaders/outline.frag");
            
            int locResolution = Raylib.GetShaderLocation(_outlineShader, "resolution");
            int locThickness = Raylib.GetShaderLocation(_outlineShader, "thickness");
            int locOutlineColor = Raylib.GetShaderLocation(_outlineShader, "outlineColor");

            // Set default values that will be updated in Draw method
            Raylib.SetShaderValue(_outlineShader, locThickness, 0.5f, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_outlineShader, locOutlineColor, new Vector4(1, 1, 1, 1), ShaderUniformDataType.Vec4);
        }
    }
    
    private Rectangle _drawRect;
    //public Rectangle DrawRect => _drawRect;
    private int _currentFrame = 0;
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; } = Color.White;
    public SpriteMode Mode { get; set; } = SpriteMode.Single;
    public bool AnimationEnabled { get; set; } = false;
    public float FramesPerSecond { get; set; } = 0f;
    private float _frameDuration => 1f / FramesPerSecond;
    private float _animationTimer = 0f;
    public Vector2 FrameSize { get; set; } = Vector2.NaN;
    
    public bool DrawDebugLines { get; set; } = false;
    
    internal bool EnableOutlineShader { get; set; }

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

    internal override void Update(float deltaTime)
    {
        if (AnimationEnabled && FramesPerSecond > 0)
        {
            _animationTimer += deltaTime;
            if (_animationTimer >= _frameDuration)
            {
                _animationTimer = 0f;
                CurrentFrame++;
            } 
        }
    }

    internal override void Draw()
    {
        if (EnableOutlineShader)
        {
            // Update resolution with actual texture dimensions
            int locResolution = Raylib.GetShaderLocation(_outlineShader, "resolution");
            Raylib.SetShaderValue(_outlineShader, locResolution, new Vector2(WorldRectangle.Width, WorldRectangle.Height), ShaderUniformDataType.Vec2);

            Raylib.BeginShaderMode(_outlineShader);
        }
        
        Raylib.DrawTexturePro(Texture, new Rectangle(CurrentFrame * _frameWidth, 0, _frameWidth, _frameHeight), WorldRectangle, PivotPoint, LocalRotation, Tint);

        if (EnableOutlineShader)
        {
            Raylib.EndShaderMode();
        }

        if (DrawDebugLines)
        {
            Raylib.DrawRectangleLinesEx(WorldRectangle, 2f, Color.Green);
        }
    }
}