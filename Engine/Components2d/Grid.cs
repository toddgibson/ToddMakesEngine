using System.Numerics;
using Raylib_cs;

namespace Engine.Components2d;

public class Grid : Component2d
{
    public int CellSize { get; set; }
    public Color BorderColor { get; set; } = Color.RayWhite;
    
    public bool DrawGridLines { get; set; } = true;

    public Grid(Vector2 size, int cellSize = 16, Color? borderColor = null)
    {
        Size = size;
        CellSize = cellSize;
        BorderColor = borderColor ?? BorderColor;
    }

    internal override void Draw()
    {
        for (var x = 0; x < Size.X; x++)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                Raylib.DrawRectangleLines((int)WorldRectangle.X + x * CellSize, (int)WorldRectangle.Y + y *  CellSize, CellSize, CellSize, BorderColor);
            }
        }
    }
}