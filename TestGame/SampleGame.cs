using Engine;
using Engine.Logging;

namespace TestGame;

public class SampleGame(GameSettings settings) : Game(settings)
{
    protected override void Initialize()
    {
        SceneManager.SceneChanged += HandleSceneChanged;

        var sampleScene = new SampleScene(this);
        var sampleScene2 = new SampleScene(this, "SampleScene2");
        
        AddScene(sampleScene, true);
        AddScene(sampleScene2);
    }

    private void HandleSceneChanged(string name, int stackSize)
    {
        Log.Info($"Active scene is now {name}. Stack size: {stackSize}", ConsoleColor.Yellow);
    }
}