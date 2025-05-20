using System.Numerics;

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
        throw new Exception("Component is not a Button");
    }
    
    public static Tween TweenRotation(this Tween tween, float to, float duration)
    {
        return new Tween(tween.Component, nameof(tween.Component.Rotation), tween.Component.Rotation, to, duration);
    }
}