using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Raylib_cs;

namespace BasicGame;

public class SimpleScene(Game game, string name = "SimpleScene") : Scene(game, name)
{
    protected override void Initialize()
    {
        // Initialize entities with components
        AddEntity(new Entity("SimpleEntity")
            {
                Position = new Vector2(64, 96),
            })
            .AddComponent2D(new Sprite()
            {
                Texture = AssetManager.GetTexture("campfire"),
                Mode = SpriteMode.Framed,
                FrameSize = Vector2.One * 64,
                CurrentFrame = 0,
                Size = new Vector2(64, 64),
                AnimationEnabled = true,
                FramesPerSecond = 10,
            });
    }

    // Optional: Do something when the scene becomes active
    protected override void Activated() 
    {
        AssetManager.GetSong("peace-song").Play();
    }

    // Optional: Do something when the scene deactivates
    protected override void Deactivated() { }

    // Optional: Do something every frame
    protected override void Update(float deltaTime) 
    {
        if (Raylib.IsKeyDown(KeyboardKey.Right))
            Game.MainCamera.Target.X += 200 * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Left))
            Game.MainCamera.Target.X -= 200 * deltaTime;
    }
}