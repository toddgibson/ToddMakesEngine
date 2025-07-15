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
                    var newCell = new GridCell()
                    {
                        Coordinate = new Vector2(x, y),
                        ScaledSize = scaledCellSize,
                        ScaledHalfSize = scaledCellHalfSize,
                        WorldPosition = cellPosition,
                        WorldCenter = cellCenter
                    };
                    Cells.Add(newCell);
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

        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                var cell = GetCellAtCoordinate(new Vector2(x, y));
                if (cell != null) 
                    CalculateNeighbors(cell);
            }
        }
    }

    public List<GridCell> CalculateNeighbors(GridCell cell)
    {
        var neighbors = new List<GridCell>();
        
        var leftNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(-1, 0));
        var rightNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(1, 0));
        var topNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(0, -1));
        var bottomNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(0, 1));
        
        var topLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(-1, -1));
        var topRightNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(1, -1));
        var bottomLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(-1, 1));
        var bottomRightNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2(1, 1));
        
        if (leftNeighbor != null)
            neighbors.Add(leftNeighbor);
        if (rightNeighbor != null)
            neighbors.Add(rightNeighbor);
        if (topNeighbor != null)
            neighbors.Add(topNeighbor);
        if (bottomNeighbor != null)
            neighbors.Add(bottomNeighbor);
        if (topLeftNeighbor != null)
            neighbors.Add(topLeftNeighbor);
        if (topRightNeighbor != null)
            neighbors.Add(topRightNeighbor);
        if (bottomLeftNeighbor != null)
            neighbors.Add(bottomLeftNeighbor);
        if (bottomRightNeighbor != null)
            neighbors.Add(bottomRightNeighbor);
        
        cell.Neighbors = neighbors;

        return neighbors;
    }
    
    public GridCell? GetCellAtCoordinate(int x, int y)
    {
        return GetCellAtCoordinate(new Vector2(x, y));
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

    internal override void Initialize()
    {
        CalculateCells();
        
        _lastGlobalPosition = GlobalPosition;
        _lastScale = Scale;
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
        
        public List<GridCell> Neighbors { get; set; } = [];
    }
}