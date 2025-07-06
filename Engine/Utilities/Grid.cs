using System.Numerics;
using Engine.Systems;
using Raylib_cs;

namespace Engine.Utilities;

public class Grid
{
    public Vector2 Size { get; set; }
    public int CellSize { get; set; }
    public Color BorderColor { get; set; } = Color.RayWhite;
    
    public bool DrawGridLines { get; set; } = true;

    public Grid(Vector2 size, int cellSize = 16, Color? borderColor = null)
    {
        Size = size;
        CellSize = cellSize;
        BorderColor = borderColor ?? BorderColor;
        
        GridSystem.Add(this);
    }

    internal void DrawInternal()
    {
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                Raylib.DrawRectangleLines(x * CellSize, y *  CellSize, CellSize, CellSize, BorderColor);
            }
        }
    }
}