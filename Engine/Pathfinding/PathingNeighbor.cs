using Engine.Numerics;

namespace Engine.Pathfinding;

public struct PathingNeighbor
{
    public PathingNeighbor() { }
    public Vector2Int GridPosition { get; set; } = Vector2Int.Zero;
    public bool IsTraversable { get; set; } = true;
}