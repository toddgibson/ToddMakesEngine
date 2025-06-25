using System.Numerics;
using Raylib_cs;

namespace Engine.Components2D;

public static class Component2dExtensions
{
    public static Tween TweenLocalPosition(this Component2D component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.LocalPosition), component.LocalPosition, to, duration);
    }
    
    public static Tween TweenLocalScale(this Component2D component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.Scale), component.Scale, to, duration);
    }
    
    public static Tween TweenLocalRotation(this Component2D component, float to, float duration)
    {
        return new Tween(component, nameof(component.LocalRotation), component.LocalRotation, to, duration);
    }
    
    public static Tween TweenLocalPosition(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component2D, nameof(tween.Component2D.LocalPosition), tween.Component2D.LocalPosition, to, duration);
    }
    
    public static Tween TweenLocalScale(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component2D, nameof(tween.Component2D.Scale), tween.Component2D.Scale, to, duration);
    }
    
    public static Tween TweenLocalRotation(this Tween tween, float to, float duration)
    {
        return new Tween(tween.Component2D, nameof(tween.Component2D.LocalRotation), tween.Component2D.LocalRotation, to, duration);
    }
    
    public static Tween TweenColorTint(this Component2D component, Color to, float duration)
    {
        if (component is Sprite sprite)
            return new Tween(sprite, nameof(sprite.Tint), sprite.Tint, to, duration);
        
        throw new Exception($"Cannot tween color. Component '{component}' is not the correct type.");
    }
    
    public static Tween TweenColorTint(this Tween tween, Color to, float duration)
    {
        if (tween.Component2D is Sprite sprite)
            return new Tween(sprite, nameof(sprite.Tint), sprite.Tint, to, duration);
        
        throw new Exception($"Cannot tween color. Component '{tween.Component2D}' is not the correct type.");
    }

    public static bool IsWithinScreenBounds(this Component2D component, int screenWidth, int screenHeight)
    {
        if (component.GlobalPosition.X + (component.Size.X * 0.5f) < 0
            || component.GlobalPosition.X - (component.Size.X * 0.5f) > screenWidth
            || component.GlobalPosition.Y + (component.Size.Y * 0.5f) < 0
            || component.GlobalPosition.Y - (component.Size.Y * 0.5f) > screenHeight)
            return false;
        
        return true;
    }
}