using Engine;

namespace TestGame;

public class SampleGame(GameSettings settings) : Game(settings)
{
    protected override void Initialize()
    {
        AssetManager.LoadTexture("Assets/sprite.png", "adventurers");
        AssetManager.LoadTexture("Assets/test-tile.png", "test-tile");
        AssetManager.LoadTexture("Assets/grass_10.png", "test-tile-hex");
        AssetManager.LoadTexture("Assets/grass_10_wall.png", "test-tile-hex-wall");
        AssetManager.LoadTexture("Assets/CampFireFinished.png", "campfire");
        AssetManager.LoadSound("Assets/clean-scream.mp3", "scream");
        AssetManager.LoadSong("Assets/dreams_of_peace_2.ogg", "peace");
        
        // Localize using resources
        AssetManager.LoadLocalizationResource("TestGame.Assets.Text");
        Console.WriteLine($@"Text Resource Value: {AssetManager.GetLocalizedText("HelloWorld")}");
        
        var sampleScene = new SampleScene(this);
        var sampleScene2 = new SampleScene(this, "SampleScene2");
        
        AddScene(sampleScene, true);
        AddScene(sampleScene2);
        
        #if DEBUG
        DisplayFrameData = true;
        #endif
    }
}