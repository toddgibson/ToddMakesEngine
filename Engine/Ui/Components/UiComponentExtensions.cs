using System.Numerics;
using Raylib_cs;

namespace Engine.Ui.Components;

public static class UiComponentExtensions
{
    public static Tween TweenPosition(this UiComponent component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.Position), component.Position, to, duration);
    }
    
    public static Tween TweenScale(this UiComponent component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.Scale), component.Scale, to, duration);
    }
    
    public static Tween TweenFontSize(this Button component, float to, float duration)
    {
        return new Tween(component, nameof(component.FontSize), component.FontSize, to, duration);
    }
    
    public static Tween TweenRotation(this UiComponent component, float to, float duration)
    {
        return new Tween(component, nameof(component.Rotation), component.Rotation, to, duration);
    }
    
    public static Tween TweenPosition(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component, nameof(tween.Component.Position), tween.Component.Position, to, duration);
    }
    
    public static Tween TweenScale(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component, nameof(tween.Component.Scale), tween.Component.Scale, to, duration);
    }
    
    public static Tween TweenFontSize(this Tween tween, float to, float duration)
    {
        if (tween.Component is Button button)
            return new Tween(button, nameof(button.FontSize), button.FontSize, to, duration);
        if (tween.Component is Label label)
            return new Tween(label, nameof(label.FontSize), label.FontSize, to, duration);
        throw new Exception($"Cannot tween font size. Component '{tween.Component}' is not the correct type.");
    }
    
    public static Tween TweenRotation(this Tween tween, float to, float duration)
    {
        return new Tween(tween.Component, nameof(tween.Component.Rotation), tween.Component.Rotation, to, duration);
    }
    
    public static Tween TweenColor(this UiComponent component, Color to, float duration)
    {
        if (component is Button button)
            return new Tween(button, nameof(button.BackgroundColor), button.BackgroundColor, to, duration);
        if (component is Label label)
            return new Tween(label, nameof(label.TextColor), label.TextColor, to, duration);
        throw new Exception($"Cannot tween color. Component '{component}' is not the correct type.");
    }
    
    public static Tween TweenColor(this Tween tween, Color to, float duration)
    {
        if (tween.Component is Button button)
            return new Tween(button, nameof(button.BackgroundColor), button.BackgroundColor, to, duration);
        if (tween.Component is Label label)
            return new Tween(label, nameof(label.TextColor), label.TextColor, to, duration);
        throw new Exception($"Cannot tween color. Component '{tween.Component}' is not the correct type.");
    }
}