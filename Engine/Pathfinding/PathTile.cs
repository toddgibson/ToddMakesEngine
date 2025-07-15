using Engine.Numerics;

namespace Engine.Pathfinding;

public class PathTile
{
    public Vector2Int GridPosition { get; set; }
    public bool IsPassable { get; set; } = true;
    public byte Weight { get; set; } = 1;

    [field: NonSerialized]
    public virtual List<Vector2Int> NeighborPositions { get; set; } = [];
}