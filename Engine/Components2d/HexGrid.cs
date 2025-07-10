namespace Engine.Components2d;

public class HexGrid : Component2d
{
    public enum Style
    {
        FlatTop,
        PointyTop
    }
    
    public Style GridStyle { get; set; } = Style.FlatTop;
    
    internal override void Draw()
    {
        
    }
}