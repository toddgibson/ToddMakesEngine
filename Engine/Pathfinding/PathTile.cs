using Engine.Numerics;

namespace Engine.Pathfinding;

public class PathTile
{
    [NonSerialized]
    private List<Vector2Int> _neighborPositions;
    public Vector2Int GridPosition { get; set; }
    public bool IsPassable { get; set; }
    public byte Weight { get; set; }

    public virtual List<Vector2Int> NeighborPositions
    {
        get => _neighborPositions;
        set => _neighborPositions = value;
    }
}