using Engine;
using Pong;

var sampleGame = new PongGame(new GameSettings()
{
    GameTitle = "Pongy"
});

EngineManager.Run(sampleGame);