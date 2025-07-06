using System.Numerics;
using Engine.Systems;
using Raylib_cs;

namespace Engine.Ui.Components;

public class Label : UiComponent
{
    public Label(string name, string text)
    {
        Name = name;
        Text = text;
        Size = Raylib.MeasureTextEx(Font, Text, FontSize, FontSpacing);
        PivotPoint = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
    }
    
    public Label(string name, string text, Color color)
    {
        Name = name;
        Text = text;
        TextColor = color;
        Size = Raylib.MeasureTextEx(Font, Text, FontSize, FontSpacing);
        PivotPoint = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
    }
    
    public Label(string name, string text, Color color, Font font)
    {
        Name = name;
        Text = text;
        TextColor = color;
        Font = font;
        Size = Raylib.MeasureTextEx(Font, Text, FontSize, FontSpacing);
        PivotPoint = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
    }
    
    public string Text { get; set; } = "";
    public Font Font { get; set; } = UiSystem.DefaultFont;
    public float FontSize { get; set; } = 24;
    public float FontSpacing { get; set; }
    public Color TextColor { get; set; } = Color.White;
    
    internal override void Draw()
    {
        Raylib.DrawTextPro(Font, Text, Position, PivotPoint, Rotation, FontSize * Scale.X, FontSpacing * Scale.X, TextColor);
    }

    internal override void Update(float delta) { }
}