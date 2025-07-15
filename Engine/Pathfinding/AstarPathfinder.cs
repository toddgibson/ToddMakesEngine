using Engine.Logging;
using Engine.Numerics;

namespace Engine.Pathfinding
{
    public class AstarPathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private readonly int PathGridWidth;
        private readonly int PathGridHeight;

        private readonly PathNode[,] PathNodes;
        private List<PathNode> OpenList;
        private HashSet<PathNode> ClosedList;
        
        private readonly Dictionary<string, List<PathNode>> _pathCache = new(); 

        public AstarPathfinder(int width, int height, List<PathTile> gridCells)
        {
            height++;
            
            PathGridWidth = width;
            PathGridHeight = height;
            PathNodes = new PathNode[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var gridCell = gridCells.FirstOrDefault(gc =>
                        gc.GridPosition == new Vector2Int(x, y) && gc.IsPassable);

                    if (gridCell != null)
                    {
                        var gridCellNeighbors = gridCell.NeighborPositions;
                        PathNodes[x, y] = new PathNode(x, y, gridCell.Weight, gridCellNeighbors);
                    }
                    else
                    {
                        PathNodes[x, y] = new PathNode(x, y, Byte.MaxValue, new List<Vector2Int>());
                    }
                }
            }
        }

        public List<PathNode> FindPath(Vector2Int gridPositionStart, Vector2Int gridPositionEnd)
        {
            return FindPath(gridPositionStart.X, gridPositionStart.Y, gridPositionEnd.X, gridPositionEnd.Y);
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            try
            {
                var pathName = $"{startX},{startY}-{endX},{endY}";
                if (_pathCache.ContainsKey(pathName))
                    return _pathCache[pathName];
                
                var startNode = PathNodes[startX, startY];
                var endNode = PathNodes[endX, endY];

                OpenList = new List<PathNode> { startNode };
                ClosedList = new HashSet<PathNode>();

                for (var x = 0; x < PathGridWidth; x++)
                {
                    for (var y = 0; y < PathGridHeight; y++)
                    {
                        var pathNode = PathNodes[x, y];
                        pathNode.gCost = int.MaxValue;
                        pathNode.CalculateFCost();
                        pathNode.previousNodeGridPosition = null;
                    }
                }

                startNode.gCost = 0;
                startNode.hCost = CalculateDistanceCost(startNode, endNode);
                startNode.CalculateFCost();

                while (OpenList.Count > 0)
                {
                    var currentNode = GetLowestFCostNode(OpenList);
                    if (currentNode == null) break;

                    if (currentNode == endNode)
                    {
                        //destination reached!
                        return CalculatePath(endNode);
                    }

                    OpenList.Remove(currentNode);
                    ClosedList.Add(currentNode);

                    foreach (var neighborNodePosition in currentNode.Neighbors)
                    {
                        var neighborNode = PathNodes[(int)neighborNodePosition.X, (int)neighborNodePosition.Y];
                        if (ClosedList.Contains(neighborNode))
                            continue;

                        var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                        if (tentativeGCost < neighborNode.gCost)
                        {
                            neighborNode.previousNodeGridPosition = currentNode.GridPosition;
                            neighborNode.gCost = tentativeGCost;
                            neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                            neighborNode.CalculateFCost();

                            if (!OpenList.Contains(neighborNode))
                                OpenList.Add(neighborNode);
                        }
                    }
                }
            }
            catch
            {
                Log.Error($"Pathfinder Error: {startX},{startY} - {endX},{endY}");
                Log.Info(PathNodes[startX, startY].ToString());
                Log.Info(PathNodes[endX, endY].ToString());
                throw;
            }

            // no path found... bummer!
            return null;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode> { endNode };
            var currentNode = endNode;
            while (currentNode.previousNodeGridPosition != null)
            {
                var previousNode = PathNodes[(int)currentNode.previousNodeGridPosition.Value.X, (int)currentNode.previousNodeGridPosition.Value.Y];
                path.Add(previousNode);
                currentNode = previousNode;
            }

            path.Reverse();
            var pathName = $"{path.First()}-{path.Last()}";
            _pathCache[pathName] = path;
            return path;
        }

        private PathNode? GetLowestFCostNode(List<PathNode> pathNodes)
        {
            var lowerMovementCostNodes = pathNodes.Where(p => p.MovementCost < byte.MaxValue).ToArray();

            return lowerMovementCostNodes.Any()
                ? lowerMovementCostNodes.OrderBy(p => p.fCost).First()
                : null;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            var actualDiagonalCost = MOVE_DIAGONAL_COST * b.MovementCost;
            var actualStraightCost = MOVE_STRAIGHT_COST * b.MovementCost;

            var xDistance = Math.Abs(a.X - b.X);
            var yDistance = Math.Abs(a.Y - b.Y);
            var remaining = Math.Abs(xDistance - yDistance);
            return actualDiagonalCost * Math.Min(xDistance, yDistance) + actualStraightCost * remaining;
        }
    }
}