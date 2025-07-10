using System.Numerics;
using Raylib_cs;
using ZLinq;

namespace Engine.Components2d;

public class Grid : Component2d
{
    public int CellSize { get; set; }
    public Color BorderColor { get; set; } = Color.RayWhite;
    
    public bool DrawGridLines { get; set; }
    public List<GridCell> Cells { get; set; } = [];

    private Vector2 _lastGlobalPosition = Vector2.NaN;
    private Vector2 _lastScale = Vector2.NaN;

    public Grid(Vector2 size, int cellSize = 16, Color? borderColor = null)
    {
        Size = size;
        CellSize = cellSize;
        BorderColor = borderColor ?? BorderColor;
    }

    public void CalculateCells()
    {
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                var scaledCellSize = new Vector2(CellSize * Scale.X, CellSize * Scale.Y);
                var scaledCellHalfSize = scaledCellSize / 2;
                var cellPosition = new Vector2(WorldRectangle.X + x * scaledCellSize.X, WorldRectangle.Y + y * scaledCellSize.Y);
                var cellCenter = new Vector2(cellPosition.X + scaledCellHalfSize.X, cellPosition.Y +  scaledCellHalfSize.Y);
                
                var cell = GetCellAtCoordinate(new Vector2(x, y));
                if (cell == null)
                {
                    Cells.Add(new GridCell()
                    {
                        Coordinate = new Vector2(x, y),
                        ScaledSize = scaledCellSize,
                        ScaledHalfSize = scaledCellHalfSize,
                        WorldPosition = cellPosition,
                        WorldCenter = cellCenter
                    });
                }
                else
                {
                    cell.ScaledSize = scaledCellSize;
                    cell.ScaledHalfSize = scaledCellHalfSize;
                    cell.WorldPosition = cellPosition;
                    cell.WorldCenter = cellCenter;
                }
            }
        }
    }

    public GridCell? GetCellAtCoordinate(Vector2 coordinate)
    {
        return Cells.AsValueEnumerable().FirstOrDefault(cell => cell.Coordinate.Equals(coordinate));
    }

    public void SetCellTexture(Vector2 coordinate, Texture2D texture)
    {
        var cell = GetCellAtCoordinate(coordinate);
        if (cell == null) return;
        
        cell.CellTexture = texture;
        cell.CellTextureRect = new Rectangle(Vector2.Zero, new Vector2(texture.Width, texture.Height));
    }

    internal override void Update(float deltaTime)
    {
        if (_lastGlobalPosition != GlobalPosition || _lastScale != Scale)
            CalculateCells();
        
        _lastGlobalPosition = GlobalPosition;
        _lastScale = Scale;
    }

    internal override void Draw()
    {
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                var cell = GetCellAtCoordinate(new Vector2(x, y));
                if (cell == null) continue;

                if (cell is { CellTexture: not null, CellTextureRect: not null })
                {
                    var drawRect = new Rectangle(cell.WorldPosition, cell.ScaledSize);
                    Raylib.DrawTexturePro(cell.CellTexture.Value, cell.CellTextureRect.Value, drawRect, Vector2.Zero, 0f, Color.White);
                }
                
                if (!DrawGridLines) continue;
                    
                Raylib.DrawRectangleLines((int)cell.WorldPosition.X, (int)cell.WorldPosition.Y, 
                    (int)cell.ScaledSize.X, (int)cell.ScaledSize.Y, BorderColor);
                Raylib.DrawCircle((int)cell.WorldCenter.X, (int)cell.WorldCenter.Y, 2 * Scale.X, BorderColor);
            }
        }
    }

    public class GridCell
    {
        public Vector2 Coordinate { get; set; }
        public Vector2 WorldPosition { get; set; }
        public Vector2 WorldCenter { get; set; }
        public Vector2 ScaledSize { get; set; }
        public Vector2 ScaledHalfSize { get; set; }
        public Texture2D? CellTexture { get; set; }
        public Rectangle? CellTextureRect { get; set; }
    }
}