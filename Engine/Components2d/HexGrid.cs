using Raylib_cs;

namespace Engine.Components2d;

public class HexGrid : Component2d
{
    public enum Style
    {
        FlatTop,
        PointyTop
    }
    
    public int CellSize { get; set; }
    public Color BorderColor { get; set; } = Color.RayWhite;
    
    public bool DrawGridLines { get; set; } = true;
    
    public Style GridStyle { get; set; } = Style.FlatTop;
    
    internal override void Draw()
    {
        
    }
}