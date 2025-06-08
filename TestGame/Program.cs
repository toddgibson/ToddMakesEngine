using Engine;
using TestGame;

var sampleGame = new SampleGame(new GameSettings()
{
    GameTitle = "Sample Game",
    //TargetFrameRate = 120
});

EngineManager.Run(sampleGame);