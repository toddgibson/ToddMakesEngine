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
        return new Tween(tween.UiComponent, nameof(tween.UiComponent.Position), tween.UiComponent.Position, to, duration);
    }
    
    public static Tween TweenScale(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.UiComponent, nameof(tween.UiComponent.Scale), tween.UiComponent.Scale, to, duration);
    }
    
    public static Tween TweenFontSize(this Tween tween, float to, float duration)
    {
        if (tween.UiComponent is Button button)
            return new Tween(button, nameof(button.FontSize), button.FontSize, to, duration);
        if (tween.UiComponent is Label label)
            return new Tween(label, nameof(label.FontSize), label.FontSize, to, duration);
        throw new Exception($"Cannot tween font size. Component '{tween.UiComponent}' is not the correct type.");
    }
    
    public static Tween TweenRotation(this Tween tween, float to, float duration)
    {
        return new Tween(tween.UiComponent, nameof(tween.UiComponent.Rotation), tween.UiComponent.Rotation, to, duration);
    }
    
    public static Tween TweenColor(this UiComponent component, Color to, float duration)
    {
        if (component is Button button)
            return new Tween(button, button.Texture.HasValue ? nameof(button.TextureTint) : nameof(button.BackgroundColor), button.Texture.HasValue ? button.TextureTint : button.BackgroundColor, to, duration);
        if (component is Label label)
            return new Tween(label, nameof(label.TextColor), label.TextColor, to, duration);
        throw new Exception($"Cannot tween color. Component '{component}' is not the correct type.");
    }
    
    public static Tween TweenColor(this Tween tween, Color to, float duration)
    {
        if (tween.UiComponent is Button button)
            return new Tween(button, button.Texture.HasValue ? nameof(button.TextureTint) : nameof(button.BackgroundColor), button.Texture.HasValue ? button.TextureTint : button.BackgroundColor, to, duration);
        if (tween.UiComponent is Label label)
            return new Tween(label, nameof(label.TextColor), label.TextColor, to, duration);
        throw new Exception($"Cannot tween color. Component '{tween.UiComponent}' is not the correct type.");
    }
}