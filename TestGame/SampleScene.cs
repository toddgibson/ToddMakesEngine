using System.Numerics;
using Engine;
using Engine.Extensions;
using Engine.Logging;
using Engine.Systems;
using Engine.Ui.Components;
using Random = Engine.Random;

namespace TestGame;

public class SampleScene(Game game, string name = "SampleScene") : Scene(game, name)
{
    private const bool UseSceneNavigation = true;

    private Label _label;
    
    protected override void Initialize()
    {
        AddUiComponent(new Button(Name == "SampleScene" ? "Spin Me!" : "Whoa Dude!", HandleButtonClickAsync));
        _label = AddUiComponent(new Label("Hello World!"));
    }

    protected override void Activated()
    {
        Log.Info($"Scene activated!", ConsoleColor.DarkGreen);
        SceneManager.SceneChanged += HandleSceneChanged;
        
        if (Name == "SampleScene")
            RunAtInterval(Simulation, 2);
    }

    protected override void Deactivated()
    {
        Log.Info($"Scene deactivated!", ConsoleColor.DarkGreen);
        SceneManager.SceneChanged -= HandleSceneChanged;
    }

    private void HandleSceneChanged(string name, int stackSize)
    {
        Log.Info($"Active scene is now {name}. Stack size: {stackSize}", ConsoleColor.Yellow);
    }

    private void Simulation()
    {
        Log.Info("simulation");
        _label.TextColor = ColorHelpers.GetRandomColor();
        _label.TweenRotation(_label.Rotation + 45, 1.0f);
    }

    private Task HandleButtonClickAsync(Button button)
    {
        button.IsEnabled = false;
        var toPosition = new Vector2(Random.Range(-(Game.Settings.ScreenWidth/2), Game.Settings.ScreenWidth/2), Random.Range(-(Game.Settings.ScreenHeight/2), Game.Settings.ScreenHeight/2));

        Log.Info($"Move to {toPosition}, Anchor {button.AnchorPoint}");
        button
            .TweenRotation(button.Rotation + 360f * 3, 1.0f)
            .TweenPosition(toPosition, 1f)
            .OnFinished(() =>
            {
                var newColor = ColorHelpers.GetRandomColor();
                button.TextureTint = newColor;
                button.HoverColor = newColor.Add(45);
                button.IsEnabled = true;
                CancelInterval(Simulation);

                if (UseSceneNavigation)
                {
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        Game.SceneManager.NavigateToScene("SampleScene2");
                    }
                    else
                    {
                        Game.SceneManager.NavigateBack();
                    }
                }
                else
                {
                    Game.SetActiveScene(Random.Range(0, 100) > 50 ? "SampleScene" : "SampleScene2");
                }
            });
        
        return Task.CompletedTask;
    }

    private Vector2 _labelVelocity = Vector2.One;
    private float _labelSpeed = 100f;
    
    protected override void Update(float deltaTime)
    {
        if (UiSystem.IsCursorHoveringUi)
            Log.Info(deltaTime, ConsoleColor.DarkMagenta);

        _label.Position += deltaTime * _labelSpeed * _labelVelocity;
        
        if (_label.Position.X >= Game.Settings.ScreenWidth || _label.Position.X <= 0)
            _labelVelocity *= new Vector2(-1, 1);
        if (_label.Position.Y >= Game.Settings.ScreenHeight || _label.Position.Y <= 0)
            _labelVelocity *= new Vector2(1, -1);
    }
}