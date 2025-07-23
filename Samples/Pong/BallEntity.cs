using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Raylib_cs;
using Random = Engine.Random;

namespace Pong;

public class BallEntity(string name = "ball") : Entity(name)
{
    public Vector2 Speed { get; set; }
    private Sprite? _player1PaddleSprite;
    private Sprite? _player2PaddleSprite;
    private Sprite _ballSprite;
    private bool _ballReady = true;
    
    public override void Initialize()
    {
        ResetBallSpeed();
        ResetPosition();

        var playerPaddles = Scene.GetEntitiesOfType<PlayerPaddleEntity>();
        _player1PaddleSprite = playerPaddles.First(p => p.Side == PlayerPaddleEntity.PlayerPaddleSideEnum.Left).GetComponentOfType<Sprite>();
        _player2PaddleSprite = playerPaddles.First(p => p.Side == PlayerPaddleEntity.PlayerPaddleSideEnum.Right).GetComponentOfType<Sprite>();

        _ballSprite = new Sprite()
        {
            Texture = AssetManager.GetTexture("ballGrey"),
            Mode = SpriteMode.Single,
            Size = new Vector2(22, 22),
            CollisionShape = CollisionShape2D.Circle,
            Tint = Color.Yellow,
            DrawDebugLines = true
        };
        AddComponent2D(_ballSprite);
    }

    public override void Update(float delta)
    {
        if (!_ballReady) return;
        
        Move(delta);
        CheckScreenEdgeCollision();
        CheckPlayerPaddleCollision();
    }

    private void Move(float delta)
    {
        var pos = Position;
        pos.X += Speed.X * delta;
        pos.Y += Speed.Y * delta;
        Position = pos;
    }

    private bool _wasCollidingWithEdgeLastFrame = false;
    private void CheckScreenEdgeCollision()
    {
        if (Position.X < 0)
        {
            HandleScoringEvent(PlayerPaddleEntity.PlayerPaddleSideEnum.Right);
        }
        else if (Position.X > Scene.Game.Settings.ScreenWidth)
        {
            HandleScoringEvent(PlayerPaddleEntity.PlayerPaddleSideEnum.Left);
        }

        var isCollidingWithEdge = Position.Y < 0 || Position.Y > Scene.Game.Settings.ScreenHeight;
        var result = isCollidingWithEdge && !_wasCollidingWithEdgeLastFrame;
        _wasCollidingWithEdgeLastFrame = isCollidingWithEdge;
        if (result)
        {
            AssetManager.GetSound("wallHitSound").Play();
            Speed = Speed with { Y = -Speed.Y };
        }
    }

    private void HandleScoringEvent(PlayerPaddleEntity.PlayerPaddleSideEnum side)
    {
        _ballReady = false;
        ResetPosition();
        GameScene.PlayerScored?.Invoke(side);
    }

    private void CheckPlayerPaddleCollision()
    {
        if (_player1PaddleSprite == null || _player2PaddleSprite == null) return;

        if (_ballSprite.CollisionJustOccuredWith(_player1PaddleSprite) || _ballSprite.CollisionJustOccuredWith(_player2PaddleSprite))
        {
            AssetManager.GetSound("paddleHitSound").Play();
            Speed = Speed with { X = -Speed.X };
            Speed *= 1.15f;
            Speed = Vector2.Clamp(Speed, new Vector2(-500, -500), new Vector2(500, 500));
        }
    }

    private void ResetPosition()
    {
        Speed = Speed with { X = -Speed.X };
        Position = new Vector2(Scene.Game.Settings.ScreenWidth / 2f, Scene.Game.Settings.ScreenHeight / 2f);
    }

    public void SetBallReady()
    {
        _ballReady = true;
    }

    public void ResetBallSpeed()
    {
        Speed = new Vector2(Random.Range(-50, 50) + 200f, Random.Range(-50, 50) + 200f);
    }
}