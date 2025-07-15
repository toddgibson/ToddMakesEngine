using Engine.Numerics;

namespace Engine.Pathfinding;

public class PathTile
{
    public Vector2Int GridPosition { get; set; }
    public bool IsPassable { get; set; }
    public byte Weight { get; set; }

    [field: NonSerialized]
    public virtual List<Vector2Int> NeighborPositions { get; set; } = [];
}