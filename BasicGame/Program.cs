using BasicGame;
using Engine;

var sampleGame = new SimpleGame(new GameSettings()
{
    GameTitle = "Simple Game",
    TargetFrameRate = 60
});

EngineManager.Run(sampleGame);