using System.Numerics;
using Engine.Systems;
using Raylib_cs;

namespace Engine.Ui.Components;

public class Button : TexturedUiComponent
{
    private Rectangle _drawRect;

    private Button()
    {
        Size = Texture.HasValue ? new Vector2(Texture.Value.Width, Texture.Value.Height) : new Vector2(150, 40);
    }
    
    public Button(string name, string text, Action<Button> clickFunc) : this()
    {
        Name = name;
        Texture = UiSystem.DefaultButtonTexture;
        PivotPoint = new Vector2(Texture.Value.Width * 0.5f, Texture.Value.Height * 0.5f);
        Text = text;
        Anchor = AnchorEnum.MiddleCenter;
        OnClick += clickFunc;
    }

    public Button(string name, Texture2D backgroundTexture, string text, Font font, Action<Button> clickFunc) : this()
    {
        Name = name;
        Texture = backgroundTexture;
        PivotPoint = new Vector2(Texture.Value.Width * 0.5f, Texture.Value.Height * 0.5f);
        Font = font;
        Text = text;
        Anchor = AnchorEnum.MiddleCenter;
        OnClick += clickFunc;
    }
    
    public Action<Button>? OnClick { get; set; }
    public string Text { get; set; } = "";
    public Vector2 TextOffset { get; set; } = Vector2.Zero;
    public Font Font { get; set; } = UiSystem.DefaultFont;
    public float FontSize { get; set; } = 24;
    public float FontSpacing { get; set; }
    
    public enum TextAlignmentEnum { Left, Center, Right }

    public TextAlignmentEnum TextAlignment { get; set; } = TextAlignmentEnum.Center;
    
    public Color TextColor { get; set; } = Color.White;
    public int TextPadding { get; set; } = 10;
    public Color BackgroundColor { get; set; } = Color.Gray;
    public Color HoverColor { get; set; } = Color.LightGray;
    public Color PressedColor { get; set; } = Color.Gray;
    public Color DisabledColor { get; set; } = Color.Gray;
    public Color DisabledTextColor { get; set; } = Color.LightGray;
    
    public bool IsEnabled { get; set; } = true;
    
    private Color _drawColor
    {
        get
        {
            if (!IsEnabled) return DisabledColor;
            if (_isPressed) return PressedColor;

            if (Texture.HasValue)
            {
                return IsHovered ? HoverColor : TextureTint;
            }

            return IsHovered ? HoverColor : BackgroundColor;
        }
    }

    private Color _textDrawColor => IsEnabled ? TextColor : DisabledTextColor;
    private bool _isPressed => IsHovered && Raylib.IsMouseButtonDown(0);

    internal override void Update(float delta)
    {
        if (!IsEnabled) return;

        if (IsHovered && Raylib.IsMouseButtonPressed(0))
        {
            OnClick?.Invoke(this);
        }
    }
    
    internal override void Draw()
    {
        if (Texture.HasValue)
        {
            _drawRect = new Rectangle(ScreenRectangle.X + PivotPoint.X, ScreenRectangle.Y + PivotPoint.Y, ScreenRectangle.Width, ScreenRectangle.Height);
            Raylib.DrawTexturePro(Texture.Value, new Rectangle(0, 0, Texture.Value.Width, Texture.Value.Height), _drawRect, PivotPoint, Rotation, _drawColor);
        }
        else
        {
            Raylib.DrawRectangleRec(ScreenRectangle, _drawColor);
        }
        
        DrawText();
    }

    private void DrawText()
    {
        var textSize = Raylib.MeasureTextEx(Font, Text, FontSize * Scale.X, FontSpacing * Scale.X);

        var textPivot = TextAlignment switch
        {
            TextAlignmentEnum.Left => new Vector2(PivotPoint.X - TextPadding, textSize.Y * 0.5f) - TextOffset,
            TextAlignmentEnum.Right => new Vector2(textSize.X - PivotPoint.X + TextPadding, textSize.Y * 0.5f) - TextOffset,
            _ => (textSize * 0.5f) - TextOffset,
        };
        
        var x = ScreenRectangle.X + textPivot.X;
        var y = ScreenRectangle.Y + textPivot.Y + (ScreenRectangle.Height - textSize.Y) / 2;

        var xOffset = TextAlignment switch
        {
            TextAlignmentEnum.Left => TextPadding,
            TextAlignmentEnum.Right => ScreenRectangle.Width - textSize.X - TextPadding,
            _ => (ScreenRectangle.Width - textSize.X) / 2
        };

        x += xOffset;
        
        var textPosition = new Vector2(x, y);
        Raylib.DrawTextPro(Font, Text, textPosition, textPivot, Rotation, FontSize * Scale.X, FontSpacing * Scale.X, _textDrawColor);
    }
}