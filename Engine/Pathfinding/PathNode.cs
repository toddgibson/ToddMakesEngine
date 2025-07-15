using Engine.Numerics;

namespace Engine.Pathfinding;

public class PathNode
{
    public readonly int X;
    public readonly int Y;
    public readonly byte MovementCost;
    public readonly List<Vector2Int> Neighbors;

    public int gCost;
    public int hCost;
    public int fCost;

    public Vector2Int? previousNodeGridPosition;
    public Vector2Int GridPosition => new(X, Y);

    public PathNode(int x, int y, byte movementCost, List<Vector2Int> neighbors)
    {
        X = x;
        Y = y;
        MovementCost = movementCost;
        Neighbors = neighbors;
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}