using System.Numerics;
using Engine.Numerics;
using Raylib_cs;
using ZLinq;

namespace Engine.Components2d;

public class HexGrid : Component2d
{
    public enum Style
    {
        FlatTop,
        PointyTop
    }

    public float CellSize { get; set; } = 1f;
    public Color BorderColor { get; set; } = Color.RayWhite;
    public bool DrawDebugLines { get; set; } = false;
    public bool DrawDebugCoordinates { get; set; } = false;
    public bool DrawDebugSidePoints { get; set; } = false;
    public Style GridStyle { get; set; } = Style.PointyTop;
    public List<HexGridCell> Cells { get; set; } = [];
    
    private Vector2 _lastGlobalPosition = Vector2.NaN;
    private Vector2 _lastScale = Vector2.NaN;
    
    private float HexWidth => (float)Math.Sqrt(3) * CellSize;
    private float HexHeight => 2f * CellSize;
    
    public HexGrid(Vector2 size, Style gridStyle = Style.PointyTop, int cellSize = 16, Color? borderColor = null)
    {
        Size = size;
        GridStyle = gridStyle;
        CellSize = cellSize;
        BorderColor = borderColor ?? BorderColor;
    }

    internal override void Initialize()
    {
        CalculateCells();
        
        _lastGlobalPosition = GlobalPosition;
        _lastScale = Scale;
    }
    
    public void CalculateCells()
    {
        Cells.Clear();
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                var coordinate = new Vector2Int(x, y);
                var worldPosition = CalculateTileWorldPosition(coordinate);
                var cell = new HexGridCell()
                {
                    Coordinate = coordinate,
                    WorldPosition = worldPosition,
                };
                for (var i = 0; i <= 5; i++)
                {
                    cell.CornerPoints.Add(CalculateCornerWorldPosition(worldPosition, i));
                }
                var cornersToDraw = cell.CornerPoints;
                cornersToDraw.Add(cornersToDraw[0]);
                cell.CornerPointsToDraw = cornersToDraw.ToArray();
                cell.CellSize = Vector2.One * CellSize;
                Cells.Add(cell);
            }
        }
        
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                var cell = GetCellAtCoordinate(new Vector2Int(x, y));
                if (cell == null) continue;
                
                CalculateNeighbors(cell);
                CalculateSideCenterPoints(cell);
            }
        }
    }

    private void CalculateSideCenterPoints(HexGridCell cell)
    {
        for (var i = 0; i < cell.Neighbors.Count; i++)
        {
            var neighbor = cell.Neighbors[i];
            cell.SideCenterPoints.Add(Raymath.Vector2Lerp(cell.WorldPosition, neighbor.WorldPosition, 0.5f));
        }
    }

    private Vector2 CalculateTileWorldPosition(Vector2Int gridPos)
    {
        if (GridStyle == Style.FlatTop)
        {
            var offset = 0f;
            if (gridPos.X % 2 != 0)
                offset = HexWidth / 2;

            var x = GlobalPosition.X + gridPos.X * HexHeight * 0.75f;
            var y = GlobalPosition.Y + gridPos.Y * HexWidth + offset;

            return new Vector2(x, y);
        }
        else
        {
            var offset = 0f;
            if (gridPos.Y % 2 != 0)
                offset = HexWidth / 2;

            var x = GlobalPosition.X + gridPos.X * HexWidth + offset;
            var y = GlobalPosition.Y + gridPos.Y * HexHeight * 0.75f;

            return new Vector2(x, y);
        }
    }

    private Vector2 CalculateCornerWorldPosition(Vector2 centerPosition, int cornerIndex)
    {
        if (cornerIndex < 0 || cornerIndex > 5) return Vector2.NaN;
        
        if (GridStyle == Style.FlatTop)
        {
            var angleDeg = 60f * cornerIndex;
            var angleRad = (float)Math.PI / 180f * angleDeg;
            return new Vector2(centerPosition.X + CellSize * (float)Math.Cos(angleRad),
                centerPosition.Y + CellSize * (float)Math.Sin(angleRad));
        }
        else
        {
            var angleDeg = 60f * cornerIndex - 30f;
            var angleRad = (float)Math.PI / 180f * angleDeg;
            return new Vector2(centerPosition.X + CellSize * (float)Math.Cos(angleRad),
                centerPosition.Y + CellSize * (float)Math.Sin(angleRad));
        }
    }
    
    public List<HexGridCell> CalculateNeighbors(HexGridCell cell)
    {
        var neighbors = new List<HexGridCell>();

        if (GridStyle == Style.PointyTop)
        {
            var isEvenRow = cell.Coordinate.Y % 2 == 0;
            
            var topLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                      (isEvenRow ? new Vector2Int(-1, -1) : new Vector2Int(0, -1)));
            var topRightNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                       (isEvenRow ? new Vector2Int(0, -1) : new Vector2Int(1, -1)));
            var bottomLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                         (isEvenRow ? new Vector2Int(-1, 1) : new Vector2Int(0, 1)));
            var bottomRightNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                          (isEvenRow ? new Vector2Int(0, 1) : new Vector2Int(1, 1)));
            var leftNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2Int(-1, 0));
            var rightNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2Int(1, 0));
            
            if (topLeftNeighbor != null)
                neighbors.Add(topLeftNeighbor);
            if (topRightNeighbor != null)
                neighbors.Add(topRightNeighbor);
            if (leftNeighbor != null)
                neighbors.Add(leftNeighbor);
            if (rightNeighbor != null)
                neighbors.Add(rightNeighbor);
            if (bottomLeftNeighbor != null)
                neighbors.Add(bottomLeftNeighbor);
            if (bottomRightNeighbor != null)
                neighbors.Add(bottomRightNeighbor);
        }
        else
        {
            var isEvenRow = cell.Coordinate.X % 2 == 0;
            
            var topLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                      (isEvenRow ? new Vector2Int(-1, -1) : new Vector2Int(-1, 0)));
            var topRightNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                       (isEvenRow ? new Vector2Int(1, -1) : new Vector2Int(1, 0)));
            var bottomLeftNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                         (isEvenRow ? new Vector2Int(-1, 0) : new Vector2Int(-1, 1)));
            var bottomRightNeighbor = GetCellAtCoordinate(cell.Coordinate + 
                                                          (isEvenRow ? new Vector2Int(1, 0) : new Vector2Int(1, 1)));
            var topNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2Int(0, -1));
            var bottomNeighbor = GetCellAtCoordinate(cell.Coordinate + new Vector2Int(0, 1));
            
            if (topLeftNeighbor != null)
                neighbors.Add(topLeftNeighbor);
            if (topRightNeighbor != null)
                neighbors.Add(topRightNeighbor);
            if (topNeighbor != null)
                neighbors.Add(topNeighbor);
            if (bottomNeighbor != null)
                neighbors.Add(bottomNeighbor);
            if (bottomLeftNeighbor != null)
                neighbors.Add(bottomLeftNeighbor);
            if (bottomRightNeighbor != null)
                neighbors.Add(bottomRightNeighbor);
        }
        
        cell.Neighbors = neighbors;

        return neighbors;
    }
    
    public HexGridCell? GetCellAtCoordinate(int x, int y)
    {
        return GetCellAtCoordinate(new Vector2Int(x, y));
    }

    public HexGridCell? GetCellAtCoordinate(Vector2Int coordinate)
    {
        return Cells.AsValueEnumerable().FirstOrDefault(cell => cell.Coordinate.Equals(coordinate));
    }
    
    public void SetCellTexture(Vector2Int coordinate, Texture2D texture)
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
        for (var i = 0; i < Cells.Count; i++)
        {
            var cell = Cells[i];
            
            if (cell is { CellTexture: not null, CellTextureRect: not null })
            {
                Raylib.DrawTexturePro(cell.CellTexture.Value, cell.CellTextureRect.Value, cell.DrawRect, Vector2.Zero, 0f, Color.White);
            }
                
            if (DrawDebugLines)
                Raylib.DrawLineStrip(cell.CornerPointsToDraw, cell.CornerPoints.Count, BorderColor);
            if (DrawDebugCoordinates)
                Raylib.DrawText($"{cell.Coordinate.X},{cell.Coordinate.Y}", (int)cell.WorldPosition.X - 5, (int)cell.WorldPosition.Y - 5, 10, BorderColor);
            if (DrawDebugSidePoints)
            {
                for (var c = 0; c < cell.SideCenterPoints.Count; c++)
                {
                    Raylib.DrawCircleV(cell.SideCenterPoints[c], 2 * Scale.X, BorderColor);
                }
            }
        }
    }
}

public class HexGridCell
{
    public Vector2Int Coordinate { get; set; }
    public Vector2 WorldPosition { get; set; }
    public List<Vector2> CornerPoints { get; set; } = [];
    internal Vector2[] CornerPointsToDraw { get; set; } = [];
    public List<Vector2> SideCenterPoints { get; set; } = [];
    public Texture2D? CellTexture { get; set; }
    public Rectangle? CellTextureRect { get; set; }
    public Vector2 CellSize { get; set; }
        
    public List<HexGridCell> Neighbors { get; set; } = [];
    public bool IsPassable { get; set; } = true;
    public byte PathWeight { get; set; } = 1;
    
    private Rectangle? _drawRect;
    internal Rectangle DrawRect
    {
        get
        {
            var size = new Vector2(CellSize.X * (float)Math.Sqrt(3), CellSize.Y * 2.02f);
            _drawRect ??= new Rectangle(WorldPosition - CellSize + new Vector2(CellSize.X / 8, 0), size);
            return _drawRect.Value;
        }
    }
}