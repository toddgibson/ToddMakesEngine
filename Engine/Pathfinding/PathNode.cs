using Engine.Numerics;

namespace Engine.Pathfinding;

public class PathNode(int x, int y, byte movementCost, List<Vector2Int> neighbors)
{
    public readonly int X = x;
    public readonly int Y = y;
    public readonly byte MovementCost = movementCost;
    public readonly List<Vector2Int> Neighbors = neighbors;

    public int GCost;
    public int HCost;
    public int FCost;

    public Vector2Int? PreviousNodeGridPosition;
    public Vector2Int GridPosition => new(X, Y);

    public override string ToString()
    {
        return $"{X},{Y}";
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
}