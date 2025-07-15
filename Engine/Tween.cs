using System.Numerics;
using System.Reflection;
using Engine.Components2d;
using Engine.Entities;
using Engine.Logging;
using Engine.Systems;
using Engine.Ui.Components;
using Raylib_cs;

namespace Engine;

public class Tween
{
    public UiComponent? UiComponent { get; }
    public Component2d? Component2d { get; }
    public Entity? Entity { get; }
    private readonly string _propertyName;
    private readonly float _startValue;
    private readonly float _targetValue;
    private readonly Vector2 _startValueVector2;
    private readonly Vector2 _targetValueVector2;
    private readonly Color _startValueColor;
    private readonly Color _targetValueColor;
    private readonly float _duration;
    
    private float _elapsed;
    
    public bool Finished { get; private set; }
    private Action? FinishedAction { get; set; }

    private readonly PropertyInfo? _propertyInfo;

    public Tween(Component2d component2d, string propertyName, float startValue, float targetValue, float duration)
    {
        Component2d = component2d;
        _propertyName = propertyName;
        _startValue = startValue;
        _targetValue = targetValue;
        _duration = duration;

        _propertyInfo = Component2d.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{component2d.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(Component2d component2d, string propertyName, Vector2 startValue, Vector2 targetValue, float duration)
    {
        Component2d = component2d;
        _propertyName = propertyName;
        _startValueVector2 = startValue;
        _targetValueVector2 = targetValue;
        _duration = duration;

        _propertyInfo = Component2d.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{component2d.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(Component2d component2d, string propertyName, Color startValue, Color targetValue, float duration)
    {
        Component2d = component2d;
        _propertyName = propertyName;
        _startValueColor = startValue;
        _targetValueColor = targetValue;
        _duration = duration;

        _propertyInfo = Component2d.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{component2d.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(UiComponent uiComponent, string propertyName, float startValue, float targetValue, float duration)
    {
        UiComponent = uiComponent;
        _propertyName = propertyName;
        _startValue = startValue;
        _targetValue = targetValue;
        _duration = duration;

        _propertyInfo = UiComponent.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{uiComponent.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(UiComponent uiComponent, string propertyName, Vector2 startValue, Vector2 targetValue, float duration)
    {
        UiComponent = uiComponent;
        _propertyName = propertyName;
        _startValueVector2 = startValue;
        _targetValueVector2 = targetValue;
        _duration = duration;

        _propertyInfo = UiComponent.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{uiComponent.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(UiComponent uiComponent, string propertyName, Color startValue, Color targetValue, float duration)
    {
        UiComponent = uiComponent;
        _propertyName = propertyName;
        _startValueColor = startValue;
        _targetValueColor = targetValue;
        _duration = duration;

        _propertyInfo = UiComponent.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{uiComponent.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(Entity entity, string propertyName, float startValue, float targetValue, float duration)
    {
        Entity = entity;
        _propertyName = propertyName;
        _startValue = startValue;
        _targetValue = targetValue;
        _duration = duration;

        _propertyInfo = Entity.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{entity.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }
    
    public Tween(Entity entity, string propertyName, Vector2 startValue, Vector2 targetValue, float duration)
    {
        Entity = entity;
        _propertyName = propertyName;
        _startValueVector2 = startValue;
        _targetValueVector2 = targetValue;
        _duration = duration;

        _propertyInfo = Entity.GetType().GetProperty(_propertyName);

        if (_propertyInfo == null)
        {
            Log.Warning($"{entity.GetType().Name}.{_propertyName} property could not be found");
            return;
        }
        
        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {propertyName} from {startValue} to {targetValue} with {duration}s duration");
        
        TweenSystem.StartTween(this);
    }

    public void Update(float delta)
    {
        if (_propertyInfo == null)
        {
            Finished = true;
            return;
        }
        
        if (Finished)
            return;

        object? objectToUpdate = UiComponent;
        if (UiComponent != null)
        {
            objectToUpdate = UiComponent;
        }
        else if (Component2d != null)
        {
            objectToUpdate = Component2d;
        }
        else if (Entity != null)
        {
            objectToUpdate = Entity;
        }
        
        if (_propertyInfo.PropertyType == typeof(float))
        {
            var newValue = Raymath.Lerp(_startValue, _targetValue, _elapsed);
            _propertyInfo.SetValue(objectToUpdate, newValue);
        }
        else if (_propertyInfo.PropertyType == typeof(Vector2))
        {
            var newValue = Raymath.Vector2Lerp(_startValueVector2, _targetValueVector2, _elapsed);
            _propertyInfo.SetValue(objectToUpdate, newValue);
        }
        else if (_propertyInfo.PropertyType == typeof(Color))
        {
            var newValue = Raylib.ColorLerp(_startValueColor, _targetValueColor, _elapsed);
            _propertyInfo.SetValue(objectToUpdate, newValue);
        }

        _elapsed += Raymath.Clamp(delta / _duration, 0f, 1f);

        if (!(_elapsed >= 1.0f)) return;
        
        Stop();
    }

    private void Stop()
    {
        Finished = true;
        
        if (_propertyInfo == null) return;

        object? finalTargetValue = null;
        object? objectToUpdate = UiComponent;

        if (UiComponent != null)
        {
            objectToUpdate = UiComponent;
        }
        else if (Component2d != null)
        {
            objectToUpdate = Component2d;
        }
        else if (Entity != null)
        {
            objectToUpdate = Entity;
        }

        try
        {
            if (_propertyInfo.PropertyType == typeof(float))
            {
                finalTargetValue = _targetValue;
            }
            else if (_propertyInfo.PropertyType == typeof(Vector2))
            {
                finalTargetValue = _targetValueVector2;
            }
            else if (_propertyInfo.PropertyType == typeof(Color))
            {
                finalTargetValue = _targetValueColor;
            }
            
            _propertyInfo.SetValue(objectToUpdate, finalTargetValue);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }

        if (TweenSystem.LoggingEnabled)
            Log.Info($"Tween {_propertyName} ending value: {_propertyInfo.GetValue(objectToUpdate)}, Target value: {finalTargetValue}");
        
        FinishedAction?.Invoke();
    }
    
    public Tween OnFinished(Action action)
    {
        FinishedAction = action;
        return this;
    }
}