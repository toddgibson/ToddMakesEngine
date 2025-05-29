using Engine;

namespace TestGame;

public class SampleGame(GameSettings settings) : Game(settings)
{
    protected override void Initialize()
    {
        var sampleScene = new SampleScene(this);
        var sampleScene2 = new SampleScene(this, "SampleScene2");
        
        AddScene(sampleScene, true);
        AddScene(sampleScene2);

        DisplayFps = true;
    }
}