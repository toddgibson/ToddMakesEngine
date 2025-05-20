using Engine;
using RaylibTest;

var sampleGame = new SampleGame(new GameOptions()
{
    GameTitle = "Sample Game",
    TargetFrameRate = 60
});

EngineManager.Run(sampleGame);