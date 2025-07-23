using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Engine.Ui.Components;
using Raylib_cs;

namespace Pong;

public class GameOverScene(Game game, string name = "gameOverScene") : Scene(game, name)
{
    private Label _winnerLabel;
    
    protected override void Initialize()
    {
        _winnerLabel = new Label("winnerLabel", "Player 1 Wins!!", Color.LightGray, AssetManager.GetFont("Kenney Blocks"))
        {
            FontSize = 120,
            Anchor = UiComponent.AnchorEnum.TopCenter,
            Position = new Vector2(-250, 150f),
            TextAlignment = Label.TextAlignmentEnum.Center,
            TextColor = Color.Orange
        };
        AddUiComponent(_winnerLabel);
        
        AddUiComponent(new Button("home", 
            AssetManager.GetTexture("buttonDefault"), "Main Menu",
            AssetManager.GetFont("Kenney Pixel Square"), 
            _ =>
            {
                AssetManager.GetSound("playSound").Play();
                Game.SceneManager.NavigateToScene("MenuScene");
            })
        {
            TextColor = Color.DarkBlue,
            Position = new Vector2(0, 50),
            HoverColor = Color.Orange,
            OnHover = (c) => { AssetManager.GetSound("mouseHover").Play(); }
        });
        
        AddEntity(new Entity("background"))
            .AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("uncolored_castle"),
                Mode = SpriteMode.Single,
                Size = new Vector2(game.Settings.ScreenWidth, game.Settings.ScreenHeight)
            });
    }

    protected override void Activated(object? payload = null)
    {
        if (payload == null)
            return;

        var sideThatWon = (PlayerPaddleEntity.PlayerPaddleSideEnum)payload;

        _winnerLabel.Text = sideThatWon == PlayerPaddleEntity.PlayerPaddleSideEnum.Left 
            ? "PLAYER 1 WINS!!" 
            : "PLAYER 2 WINS!!";
    }
}