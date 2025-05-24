using System.Numerics;
using Engine;
using Engine.Extensions;
using Engine.Logging;
using Engine.Systems;
using Engine.Ui.Components;
using Raylib_cs;

namespace RaylibTest;

public class SampleGame(GameOptions gameOptions) : Game(gameOptions)
{
    private Button _testButton;

    public override void Initialize()
    {
        base.Initialize();

        _testButton = new Button("Spin Me!", HandleButtonClickAsync);
    }
    
    private Task HandleButtonClickAsync()
    {
        _testButton.IsEnabled = false;
        var toPosition = new Vector2(Raylib.GetRandomValue(-(GameOptions.ScreenWidth/2), GameOptions.ScreenWidth/2), Raylib.GetRandomValue(-(GameOptions.ScreenHeight/2), GameOptions.ScreenHeight/2));

        Log.Info($"Move to {toPosition}, Anchor {_testButton.AnchorPoint}");
        _testButton
            .TweenRotation(_testButton.Rotation + 360f * 3, 1.0f)
            .TweenPosition(toPosition, 1f)
            .OnFinished(() =>
            {
                _testButton.IsEnabled = true;
                var newColor = ColorHelpers.GetRandomColor();
                _testButton.TextureTint = newColor;
                _testButton.HoverColor = newColor.Add(45);
            });
        
        return Task.CompletedTask;
    }
}