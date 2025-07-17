using System.Numerics;
using Raylib_cs;

namespace Engine.Components2d;

public static class Component2dExtensions
{
    public static Tween TweenLocalPosition(this Component2d component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.LocalPosition), component.LocalPosition, to, duration);
    }
    
    public static Tween TweenLocalScale(this Component2d component, Vector2 to, float duration)
    {
        return new Tween(component, nameof(component.Scale), component.Scale, to, duration);
    }
    
    public static Tween TweenLocalRotation(this Component2d component, float to, float duration)
    {
        return new Tween(component, nameof(component.LocalRotation), component.LocalRotation, to, duration);
    }
    
    public static Tween TweenLocalPosition(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component2d, nameof(tween.Component2d.LocalPosition), tween.Component2d.LocalPosition, to, duration);
    }
    
    public static Tween TweenLocalScale(this Tween tween, Vector2 to, float duration)
    {
        return new Tween(tween.Component2d, nameof(tween.Component2d.Scale), tween.Component2d.Scale, to, duration);
    }
    
    public static Tween TweenLocalRotation(this Tween tween, float to, float duration)
    {
        return new Tween(tween.Component2d, nameof(tween.Component2d.LocalRotation), tween.Component2d.LocalRotation, to, duration);
    }
    
    public static Tween TweenColorTint(this Component2d component, Color to, float duration)
    {
        if (component is Sprite sprite)
            return new Tween(sprite, nameof(sprite.Tint), sprite.Tint, to, duration);
        
        throw new Exception($"Cannot tween color. Component '{component}' is not the correct type.");
    }
    
    public static Tween TweenColorTint(this Tween tween, Color to, float duration)
    {
        if (tween.Component2d is Sprite sprite)
            return new Tween(sprite, nameof(sprite.Tint), sprite.Tint, to, duration);
        
        throw new Exception($"Cannot tween color. Component '{tween.Component2d}' is not the correct type.");
    }

    public static bool IsWithinVisibleBounds(this Component2d component, Rectangle cameraBounds)
    {
        return Raylib.CheckCollisionRecs(component.WorldRectangle, cameraBounds);
    }
}