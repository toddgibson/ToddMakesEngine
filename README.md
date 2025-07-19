# ToddMakesEngine 

This is a simple 2D game engine built with C# and [Raylib](https://github.com/raysan5/raylib). The primary goal is to include enough tooling within the engine to enable building games in a fast and efficient manner.

This project is available under the MIT license. 
[Raylib](https://github.com/raysan5/raylib) and the [Raylib_cs](https://github.com/raylib-cs/raylib-cs) language bindings are available under their respective licenses.

---

## Dependencies

- .Net 9.0+
- [Raylib_cs](https://github.com/raylib-cs/raylib-cs) - Raylib C# bindings
- [Zlinq](https://github.com/Cysharp/ZLinq) - Zero-allocation Linq

## Features

- Entities/Components Based, organized in Scenes
- 2D sprite rendering with animation support
- Asset management
- Scene management
- Grid components (square & hex)
- Pathfinding (A* algorithm)
- UI components with anchoring
- Sound effects and music playback
- Camera controls
- Tweening system for smooth animations
- GLSL Shader support

---

## Getting Started

### 1. Create a C# console app project 
### 2. Add a reference to the engine library
### 3. Create a Game class
```csharp
public class SampleGame(GameSettings settings) : Game(settings)
{
    protected override void Initialize()
    {
        // Load assets
        AssetManager.LoadTexture("Assets/campfire.png", "campfire");
        AssetManager.LoadSound("Assets/scream.mp3", "scream-sfx");
        AssetManager.LoadSong("Assets/peace.ogg", "peace-song");
        
        // Initialize your games Scenes
        AddScene(new SimpleScene(this), true);
            
        // Optionally, display debug data
        #if DEBUG
        DisplayFrameData = true;
        #endif
    }
}
```
### 4. Create a Scene class
```csharp
public class SimpleScene(Game game, string name = "SimpleScene") : Scene(game, name)
{
    protected override void Initialize()
    {
        // Initialize entities with components
        AddEntity(new Entity("SimpleEntity"))
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
```
### 5. Call Run from *Program.cs*
```csharp
using Engine; 
using TestGame;

var sampleGame = new SampleGame(new GameSettings()
{
    GameTitle = "Sample Game",
    TargetFrameRate = 60
});

EngineManager.Run(sampleGame);
```

## Assets
In your *.csproj* file, make sure asset folders and files are set to copy during the build, or they will not load properly.
```xml
<ItemGroup>
  <Folder Include="Assets\" />
</ItemGroup>

<ItemGroup>
  <None Remove="Assets\campfire.png" />
  <Content Include="Assets\campfire.png">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

---

## 2D Components

| Current      | Planned         |
|--------------|-----------------|
| Grid         | Isometric Grid  |
| Hex Grid     | Particle Effect |
| Sprite       |                 |
| Sound Effect |                 |
| Song         |                 |

---

## UI Components

| Current      | Planned         |
|--------------|-----------------|
| Button       | Panel           |
| Label        | Slider          |
|              | Toggle          |
|              | Progress Bar    |

---

## More Samples

Look at the sample projects for reference on how to set up scenes, entities, and components.
The TestGame project demonstrates the engine's capabilities through the following features:

### Scene Setup
- Interactive UI with a spinning button
- Bouncing "Hello World" label
- Multiple animated sprite characters
- Grid-based movement demonstration
- Hexagonal grid pathfinding
- Campfire animated sprite

### Controls
- Arrow keys (Left/Right) to move the camera
- Click the "Spin Me!" button to trigger animations and scene transitions

---