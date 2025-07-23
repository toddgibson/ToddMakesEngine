using Engine;

namespace Pong;

public class PongGame(GameSettings settings) : Game(settings)
{
    protected override void Initialize()
    {
        // Load assets
        AssetManager.LoadAllTextures();
        AssetManager.LoadAllFonts();
        
        AssetManager.LoadSound("Assets/Sounds/rollover4.ogg", "mouseHover");
        AssetManager.LoadSound("Assets/Sounds/bong_001.ogg", "quitSound");
        AssetManager.LoadSound("Assets/Sounds/confirmation_002.ogg", "playSound");
        AssetManager.LoadSound("Assets/Sounds/forceField_001.ogg", "paddleHitSound");
        AssetManager.LoadSound("Assets/Sounds/impactMetal_002.ogg", "wallHitSound");
        
        AssetManager.LoadSong("Assets/Songs/song1.wav");
        AssetManager.LoadSong("Assets/Songs/song2.wav");
        AssetManager.LoadSong("Assets/Songs/song3.wav");
        AssetManager.LoadSong("Assets/Songs/song4.wav");
        AssetManager.LoadSong("Assets/Songs/song5.wav");
        
        // Initialize your games Scenes
        AddScene(new MainMenuScene(this), true);
        AddScene(new GameScene(this));
        AddScene(new GameOverScene(this));
            
        // Optionally, display debug data
#if DEBUG
        DisplayFrameData = true;
#endif
    }
}