using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Engine.Logging;
using Engine.Systems;
using Engine.Ui.Components;
using Raylib_cs;
using Random = Engine.Random;

namespace Pong;

public class MainMenuScene(Game game, string name = "MenuScene") : Scene(game, name)
{
    protected override void Initialize()
    {
        var hoverSound = AssetManager.GetSound("mouseHover");
        
        // Initialize entities with components
        AddUiComponent(new Label("title", "PONGY", Color.LightGray, AssetManager.GetFont("Kenney Blocks"))
        {
            FontSize = 200,
            Anchor = UiComponent.AnchorEnum.TopCenter,
            Position = new Vector2(-200, 100f),
            TextAlignment = Label.TextAlignmentEnum.Center,
            TextColor = Color.Orange
        });
        
        AddUiComponent(new Button("play", 
            AssetManager.GetTexture("buttonDefault"), "Play",
            AssetManager.GetFont("Kenney Pixel Square"), _ =>
            {
                AssetManager.GetSound("playSound").Play();
                Game.SceneManager.NavigateToScene("GameScene");
            })
        {
            TextColor = Color.DarkBlue,
            HoverColor = Color.Orange,
            OnHover = (c) => { hoverSound.Play(); }
        });
        
        AddUiComponent(new Button("quit", 
            AssetManager.GetTexture("buttonDefault"), "Quit",
            AssetManager.GetFont("Kenney Pixel Square"), 
            _ =>
            {
                AssetManager.GetSound("quitSound").Play();
                Game.Quit();
            })
        {
            TextColor = Color.DarkBlue,
            Position = new Vector2(0, 50),
            HoverColor = Color.Orange,
            OnHover = (c) => { hoverSound.Play(); }
        });
        
        AddUiComponent(new Button("volumetest", 
            AssetManager.GetTexture("buttonDefault"), "Lower Volume",
            AssetManager.GetFont("Kenney Pixel Square"), 
            (button) =>
            {
                AudioMasterSystem.Volume = AudioMasterSystem.Volume.Equals(1f) ? 0.15f : 1f;
                button.Text = button.Text.Equals("Increase Volume") ? "Lower Volume" : "Increase Volume";
            })
        {
            TextColor = Color.DarkBlue,
            Position = new Vector2(-25, 300),
            HoverColor = Color.Orange,
            OnHover = (c) => { hoverSound.Play(); },
            Size = new Vector2(200, 40),
        });

        AddUiComponent(new Label("footer", "Made with ToddMakesEngine", Color.DarkGray,
            AssetManager.GetFont("Kenney Pixel Square"))
        {
            Anchor = UiComponent.AnchorEnum.BottomCenter,
            TextAlignment = Label.TextAlignmentEnum.Center,
            Position = new Vector2(150, 0f),
            FontSize = 20
        });

        AddEntity(new Entity("background"))
            .AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("uncolored_castle"),
                Mode = SpriteMode.Single,
                Size = new Vector2(game.Settings.ScreenWidth, game.Settings.ScreenHeight)
            });
    }

    // Optional: Do something when the scene becomes active
    protected override void Activated(object? payload = null)
    {
        AssetManager.GetSong($"song{Random.Range(1,5)}").Play();
    }

    // Optional: Do something when the scene deactivates
    protected override void Deactivated() { }

    protected override void Update(float deltaTime)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Game.Quit();
        }
    }
}