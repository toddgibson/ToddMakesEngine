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

    protected override void Initialize()
    {
        AddUiComponent(new Button(Name == "SampleScene" ? "Spin Me!" : "Whoa Dude!", HandleButtonClickAsync));
    }

    protected override void WhenBecomesActive()
    {
        if (Name == "SampleScene")
            RunAtInterval(Simulation, 2);
    }

    private void Simulation()
    {
        Log.Info("simulation");
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

    protected override void Update(float deltaTime)
    {
        if (UiSystem.IsCursorHoveringUi)
            Log.Info(deltaTime, ConsoleColor.DarkMagenta);
    }
}