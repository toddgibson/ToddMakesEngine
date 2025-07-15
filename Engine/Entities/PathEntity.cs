using Engine.Pathfinding;

namespace Engine.Entities;

public class PathEntity(string name) : Entity(name)
{
    public int CurrentPathNodeIndex { get; set; }
    public PathNode CurrentPathNode => Path[CurrentPathNodeIndex];
    public List<PathNode> Path { get; private set; } = [];
    public bool HasPath => Path.Count != 0 && CurrentPathNodeIndex < Path.Count - 1;
    public PathNode? GetNextPathNode()
    {
        if (!HasPath) return null;
        CurrentPathNodeIndex++;
        return CurrentPathNodeIndex >= Path.Count ? null : Path[CurrentPathNodeIndex];
    }

    public void SetPath(List<PathNode>? path)
    {
        CurrentPathNodeIndex = 0;
        if (path == null)
        {
            Path = [];
            return;
        }
        Path = path;
    }
}