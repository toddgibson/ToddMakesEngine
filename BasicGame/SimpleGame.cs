using Engine;

namespace BasicGame;

public class SimpleGame(GameSettings settings) : Game(settings)
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