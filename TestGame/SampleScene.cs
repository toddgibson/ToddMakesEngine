using System.Numerics;
using Engine;
using Engine.Components2d;
using Engine.Entities;
using Engine.Extensions;
using Engine.Logging;
using Engine.Numerics;
using Engine.Pathfinding;
using Engine.Systems;
using Engine.Ui.Components;
using Random = Engine.Random;

namespace TestGame;

public class SampleScene(Game game, string name = "SampleScene") : Scene(game, name)
{
    private const bool UseSceneNavigation = true;

    private Label _label;
    private SoundEffect _soundEffect;
    private Song _song; 
    
    private Vector2 _labelVelocity = Vector2.One;
    private float _labelSpeed = 100f;
    
    private Grid _grid;
    private AstarPathfinder _pathFinder;
    private PathEntity _pathCharacter;
    private Vector2Int _pathCharacterGridPosition;
    
    private HexGrid _hexGrid;
    private AstarPathfinder _hexPathFinder;
    private PathEntity _hexPathCharacter;
    private Vector2Int _hexPathCharacterGridPosition;
    
    protected override void Initialize()
    {
        AddUiComponent(new Button("btnSpin", Name == "SampleScene" ? "Spin Me!" : "Whoa Dude!", HandleButtonClickAsync));
        _label = AddUiComponent(new Label("lblHello", "Hello World!"));

        var spriteTexture = AssetManager.GetTexture("adventurers");
        var scream = AssetManager.GetSound("scream");
        var tileTexture = AssetManager.GetTexture("test-tile");
        _soundEffect = new SoundEffect(scream);
        _song = new Song(AssetManager.GetSong("peace"));
        

        // add entity...
        // with a sprite component and a sound player component
        var e = AddEntity(new Entity("Entity1")
            {
                Position = new Vector2(400, 400)
            })
            .AddComponent2D(new Sprite(true)
            {
                Texture = spriteTexture,
                Mode = SpriteMode.Framed,
                FrameSize = Vector2.One * 16,
                CurrentFrame = 0,
                LocalPosition = Vector2.Zero,
                PivotPoint = new Vector2(0.5f * 16, 0.5f * 16),
                Size = new Vector2(16, 16),
                Scale = Vector2.One * 3,
                DrawLayer = 999
            })
            .AddComponent2D(new Sprite(false)
            {
                Texture = spriteTexture,
                Mode = SpriteMode.Framed,
                FrameSize = Vector2.One * 16,
                CurrentFrame = 1,
                LocalPosition = new Vector2(16, 0) * 3,
                PivotPoint = new Vector2(0.5f * 16, 0.5f * 16),
                Size = new Vector2(16, 16),
                Scale = Vector2.One * 3,
                DrawLayer = 999
            });
            
        _grid = new Grid(new Vector2(8, 8), 16 * 3)
        {
            Scale = Vector2.One * 0.75f
        };
        AddEntity(new Entity("GridEntity")
            {
                Position = new Vector2(64f, 80f)
            }) 
            .AddComponent2D(_grid);

        foreach (var cell in _grid.Cells)
        {
            _grid.SetCellTexture(cell.Coordinate, tileTexture);
        }
        _pathFinder = new AstarPathfinder((int)_grid.Size.X, (int)_grid.Size.Y, _grid.Cells);

        _pathCharacter = AddEntity(new PathEntity("PathCharacter")
        {
            Position = _grid.GetCellAtCoordinate(0, 0)?.WorldCenter ?? Vector2.Zero,
            CurrentPathNodeIndex = 0
        });
        _pathCharacter.AddComponent2D(new Sprite()
        {
            Texture = spriteTexture,
            Mode = SpriteMode.Framed,
            FrameSize = Vector2.One * 16,
            CurrentFrame = 5,
            LocalPosition = new Vector2(-4, -12) * 3,
            PivotPoint = new Vector2(0.5f * 16, 0.5f * 16),
            Size = new Vector2(16, 16),
            Scale = Vector2.One * 3
        });
        _pathCharacterGridPosition = Vector2Int.Zero;

        _hexGrid = new HexGrid(new Vector2(8, 8), HexGrid.Style.FlatTop, 24);
        AddEntity(new Entity("HexGridEntity")
            {
                Position = new Vector2(464f, 80f)
            })
            .AddComponent2D(_hexGrid);
        
        _hexPathFinder = new AstarPathfinder((int)_hexGrid.Size.X, (int)_hexGrid.Size.Y, _hexGrid.Cells);

        _hexPathCharacter = AddEntity(new PathEntity("HexPathCharacter")
        {
            Position = _hexGrid.GetCellAtCoordinate(0, 0)?.WorldPosition ?? Vector2.Zero,
            CurrentPathNodeIndex = 0
        });
        _hexPathCharacter.AddComponent2D(new Sprite()
        {
            Texture = spriteTexture,
            Mode = SpriteMode.Framed,
            FrameSize = Vector2.One * 16,
            CurrentFrame = 6,
            LocalPosition = new Vector2(-4, -12) * 3,
            PivotPoint = new Vector2(0.5f * 16, 0.5f * 16),
            Size = new Vector2(16, 16),
            Scale = Vector2.One * 3
        });
        _hexPathCharacterGridPosition = Vector2Int.Zero;

        // for (var i = 0; i < 10000; i++)
        // {
        //     e.AddComponent2D(new Sprite()
        //     {
        //         Texture = spriteTexture,
        //         Mode = SpriteMode.Framed,
        //         FrameSize = Vector2.One * 16,
        //         CurrentFrame = Random.Range(0, 4),
        //         LocalPosition = Random.Vector2(-250, 250),
        //         PivotPoint = new Vector2(0.5f * 16, 0.5f * 16),
        //         Size = new Vector2(16, 16),
        //         Scale = Vector2.One * 3
        //     });
        // }
    }

    protected override void Activated()
    {
        Log.Info($"Scene {Name} activated!", ConsoleColor.DarkGreen);
        SceneManager.SceneChanged += HandleSceneChanged;
        
        //_song.Play();
        
        if (Name == "SampleScene")
            RunAtInterval(Simulation, 2);
    }

    protected override void Deactivated()
    {
        Log.Info($"Scene {Name} deactivated!", ConsoleColor.DarkGreen);
        SceneManager.SceneChanged -= HandleSceneChanged;
    }

    private void HandleSceneChanged(string name, int stackSize)
    {
        Log.Info($"Active scene is now {name}. Stack size: {stackSize}", ConsoleColor.Yellow);
    }

    private void Simulation()
    {
        Log.Info($"simulation");
        
        _label
            .TweenColor(ColorHelpers.GetRandomColor(), 1.0f)
            .TweenRotation(_label.Rotation + 45, 1.0f);

        var spriteEntity = GetEntitiesOfType<Entity>().FirstOrDefault();
        if (spriteEntity != null)
        {
            spriteEntity.GetComponentOfType<Sprite>()!.CurrentFrame += 1;
            spriteEntity.GetComponentsOfType<Sprite>()[1].TweenColorTint(ColorHelpers.GetRandomColor(), 1.0f);
        }

        // move the grid character around
        if (!_pathCharacter.HasPath)
        {
            var randomTile = _grid.Cells.Where(p => p.IsPassable).ToList().PickRandom();
            var path = _pathFinder.FindPath(_pathCharacterGridPosition, randomTile.Coordinate.ToVector2Int());
            _pathCharacter.SetPath(path);
        }
        else
        {
            var nextPathNodeWorldPosition = _grid.GetCellAtCoordinate(_pathCharacter.GetNextPathNode()!.ToVector2())!.WorldCenter;
            new Tween(_pathCharacter, nameof(_pathCharacter.Position), _pathCharacter.Position,
                nextPathNodeWorldPosition, 1f).OnFinished(() =>
            {
                Log.Info($"Path: {_pathCharacter.CurrentPathNodeIndex}/{_pathCharacter.Path.Count - 1}");
                _pathCharacterGridPosition = _pathCharacter.CurrentPathNode.GridPosition;
            });
        }
        
        // move the hex grid character around
        if (!_hexPathCharacter.HasPath)
        {
            var randomTile = _hexGrid.Cells.Where(p => p.IsPassable).ToList().PickRandom();
            var path = _hexPathFinder.FindPath(_hexPathCharacterGridPosition, randomTile.Coordinate);
            _hexPathCharacter.SetPath(path);
        }
        else
        {
            var nextPathNodeWorldPosition = _hexGrid.GetCellAtCoordinate(_hexPathCharacter.GetNextPathNode()!.ToVector2Int())!.WorldPosition;
            new Tween(_hexPathCharacter, nameof(_hexPathCharacter.Position), _hexPathCharacter.Position,
                nextPathNodeWorldPosition, 1f).OnFinished(() =>
            {
                Log.Info($"Hex Path: {_hexPathCharacter.CurrentPathNodeIndex}/{_hexPathCharacter.Path.Count - 1}");
                _hexPathCharacterGridPosition = _hexPathCharacter.CurrentPathNode.GridPosition;
            });
        }
    }

    private Task HandleButtonClickAsync(Button button)
    {
        var toPosition = new Vector2(
            Random.Range(-(Game.Settings.ScreenWidth / 2), Game.Settings.ScreenWidth / 2),
            Random.Range(-(Game.Settings.ScreenHeight / 2), Game.Settings.ScreenHeight / 2));

        Log.Info($"Move to {toPosition}, Anchor {button.AnchorPoint}");
        
        //_soundEffect.Play();
        
        var newColor = ColorHelpers.GetRandomColor();
        button
            .TweenRotation(button.Rotation + 360f * 3, 1.0f)
            .TweenPosition(toPosition, 1f)
            .TweenColor(newColor, 1.0f)
            .OnFinished(() =>
            {
                button.HoverColor = newColor.Add(45);
                CancelInterval(Simulation);

                if (UseSceneNavigation)
                {
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        Game.SceneManager.NavigateToScene("SampleScene2");
                    }
                    else
                    {
                        Game.SceneManager.NavigateBack();
                    }
                }
                else
                {
                    Game.SetActiveScene(Random.Range(0, 100) > 50 ? "SampleScene" : "SampleScene2");
                }
            });
        
        return Task.CompletedTask;
    }

    protected override void Update(float deltaTime)
    {
        if (UiSystem.IsCursorHoveringUi)
            Log.Info($"Hovering: {UiSystem.HoveredUiComponent?.Name} - {deltaTime}", ConsoleColor.DarkMagenta);

        _label.Position += deltaTime * _labelSpeed * _labelVelocity;
        
        if (_label.Position.X >= Game.Settings.ScreenWidth || _label.Position.X < 0)
            _labelVelocity *= new Vector2(-1, 1);
        if (_label.Position.Y >= Game.Settings.ScreenHeight || _label.Position.Y < 0)
            _labelVelocity *= new Vector2(1, -1);
        
        var entity = GetEntitiesOfType<Entity>().FirstOrDefault();
        entity.Position = _label.Position;

        var spriteComponents = entity.GetComponentsOfType<Sprite>();
        spriteComponents.ElementAt(Random.Range(0, spriteComponents.Count - 1)).DrawLayer++;
    }
}