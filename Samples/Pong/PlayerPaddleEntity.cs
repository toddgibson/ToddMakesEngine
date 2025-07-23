using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Raylib_cs;

namespace Pong;

public class PlayerPaddleEntity(string name, PlayerPaddleEntity.PlayerPaddleSideEnum side) : Entity(name)
{
    public enum PlayerPaddleSideEnum { Left, Right }
    
    public float Speed { get; set; } = 200f;
    private float _heightBound = Raylib.GetScreenHeight() - 104f; 
    public PlayerPaddleSideEnum Side { get; set; } = side;
    
    public override void Initialize()
    {
        var xPos = Side == PlayerPaddleSideEnum.Left ? 100 : Raylib.GetScreenWidth() - 100f;
        Position = new Vector2(xPos, (Raylib.GetScreenHeight() / 2f) - 52);
        
        if (Side == PlayerPaddleSideEnum.Left)
        {
            AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("paddleBlu_vert"),
                Mode = SpriteMode.Single,
                Size = new Vector2(24, 104),
                CollisionShape = CollisionShape2D.Rectangle
            });
        }
        else
        {
            AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("paddleRed_vert"),
                Mode = SpriteMode.Single,
                Size = new Vector2(24, 104),
                CollisionShape = CollisionShape2D.Rectangle
            });
        }
    }

    public override void Update(float deltaTime)
    {
        var position = Position;

        if (Side == PlayerPaddleSideEnum.Left)
        {
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                position.Y -= deltaTime * Speed;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                position.Y += deltaTime * Speed;
            }
        }
        else
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                position.Y -= deltaTime * Speed;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                position.Y += deltaTime * Speed;
            }
        }
        
        if (position.Y < 0)
            position.Y = 0;
        if (position.Y > _heightBound)
            position.Y = _heightBound;

        Position = position;
    }
}