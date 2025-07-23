using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Engine.Ui.Components;
using Raylib_cs;
using Random = Engine.Random;

namespace Pong;

public class GameScene(Game game, string name = "GameScene") : Scene(game, name)
{
    private int _player1Score;
    private int _player2Score;
    
    private Label _player1Label;
    private Label _player2Label;

    public static Action<PlayerPaddleEntity.PlayerPaddleSideEnum>? PlayerScored;
    
    protected override void Initialize()
    {
        AddEntity(new Entity("background"))
            .AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("colored_castle"),
                Mode = SpriteMode.Single,
                Size = new Vector2(game.Settings.ScreenWidth, game.Settings.ScreenHeight)
            });

        AddEntity(new PlayerPaddleEntity("Player 1", PlayerPaddleEntity.PlayerPaddleSideEnum.Left));
        AddEntity(new PlayerPaddleEntity("Player 2", PlayerPaddleEntity.PlayerPaddleSideEnum.Right));
        AddEntity(new BallEntity());

        _player1Label = new Label("player1Score", "Player 1: 0", Color.DarkBlue, AssetManager.GetFont("Kenney Blocks"))
        {
            Anchor = UiComponent.AnchorEnum.TopLeft,
            Position = new Vector2(200, 40),
            FontSize = 48
        };

        _player2Label = new Label("player2Score", "Player 2: 0", Color.Red, AssetManager.GetFont("Kenney Blocks"))
        {
            Anchor = UiComponent.AnchorEnum.TopRight,
            Position = new Vector2(-200, 40),
            FontSize = 48
        };
        
        AddUiComponent(_player1Label);
        AddUiComponent(_player2Label);
    }

    private void HandlePlayerScored(PlayerPaddleEntity.PlayerPaddleSideEnum sideThatScored)
    {
        AssetManager.GetSound("playSound").Play();
        
        if (sideThatScored == PlayerPaddleEntity.PlayerPaddleSideEnum.Left)
        {
            _player1Score++;
            _player1Label.Text = $"Player 1: {_player1Score}";
        }
        else
        {
            _player2Score++;
            _player2Label.Text = $"Player 2: {_player2Score}";
        }

        if (_player1Score >= 10 || _player2Score >= 10)
        {
            Game.SceneManager.NavigateToScene("gameOverScene", sideThatScored);
        }

        RunAtInterval(() => { GetFirstEntityOfType<BallEntity>().SetBallReady(); }, 2f, true);
    }

    protected override void Activated(object? payload = null)
    {
        AssetManager.GetSong($"song{Random.Range(1,5)}").Play();
        PlayerScored += HandlePlayerScored;
    }

    protected override void Deactivated()
    {
        PlayerScored -= HandlePlayerScored;
        
        // reset
        _player1Score = 0;
        _player2Score = 0;
        _player1Label.Text = $"Player 1: {_player1Score}";
        _player2Label.Text = $"Player 2: {_player2Score}";
        GetFirstEntityOfType<BallEntity>().ResetBallSpeed();
    }

    protected override void Update(float deltaTime)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Game.SceneManager.NavigateBack();
        }
    }
}