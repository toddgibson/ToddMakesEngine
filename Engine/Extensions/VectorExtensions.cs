using System.Numerics;
using Engine.Numerics;
using Engine.Pathfinding;

namespace Engine.Extensions;

public static class VectorExtensions
{
    public static Vector2Int ToVector2Int(this Vector2 vector)
    {
        return new Vector2Int((int)vector.X, (int)vector.Y);
    }
    
    public static Vector2 ToVector2(this Vector2Int vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    public static Vector2Int ToVector2Int(this PathNode pathNode)
    {
        return new Vector2Int(pathNode.X, pathNode.Y);
    }
    
    public static Vector2 ToVector2(this PathNode pathNode)
    {
        return new Vector2(pathNode.X, pathNode.Y);
    }
}