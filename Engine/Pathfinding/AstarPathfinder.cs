using System.Numerics;
using Engine.Components2d;
using Engine.Extensions;
using Engine.Logging;
using Engine.Numerics;
using ZLinq;

namespace Engine.Pathfinding
{
    public class AstarPathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private int _pathGridWidth;
        private int _pathGridHeight;

        private PathNode[,] _pathNodes;
        private List<PathNode> _openList = [];
        private HashSet<PathNode> _closedList = [];
        
        private readonly Dictionary<string, List<PathNode>> _pathCache = new();

        public AstarPathfinder(int width, int height, List<Grid.GridCell> gridCells)
        {
            var pathTiles = new List<PathTile>();
            for (var i = 0; i < gridCells.Count; i++)
            {
                var cell  = gridCells[i];
                pathTiles.Add(new PathTile()
                {
                    GridPosition = cell.Coordinate.ToVector2Int(),
                    NeighborPositions = cell.Neighbors.Select(p => new PathingNeighbor { GridPosition = p.Coordinate.ToVector2Int() }).ToList(),
                    IsPassable = cell.IsPassable,
                    Weight = cell.PathWeight
                });
            }
            Initialize(width, height, pathTiles);
        }
        
        public AstarPathfinder(int width, int height, List<HexGridCell> gridCells)
        {
            var pathTiles = new List<PathTile>();
            for (var i = 0; i < gridCells.Count; i++)
            {
                var cell  = gridCells[i];
                pathTiles.Add(new PathTile()
                {
                    GridPosition = cell.Coordinate,
                    NeighborPositions = cell.Neighbors.Select(p => new PathingNeighbor { GridPosition = p.Coordinate }).ToList(),
                    IsPassable = cell.IsPassable,
                    Weight = cell.PathWeight
                });
            }
            Initialize(width, height, pathTiles);
        }
        
        public AstarPathfinder(int width, int height, List<PathTile> gridCells)
        {
            Initialize(width, height, gridCells);
        }

        private void Initialize(int width, int height, List<PathTile> gridCells)
        {
            height++;
            
            _pathGridWidth = width;
            _pathGridHeight = height;
            _pathNodes = new PathNode[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var gridCell = gridCells.FirstOrDefault(gc =>
                        gc.GridPosition == new Vector2Int(x, y) && gc.IsPassable);

                    if (gridCell != null)
                    {
                        var gridCellNeighbors = gridCell.NeighborPositions;
                        _pathNodes[x, y] = new PathNode(x, y, gridCell.Weight, gridCellNeighbors);
                    }
                    else
                    {
                        _pathNodes[x, y] = new PathNode(x, y, byte.MaxValue, []);
                    }
                }
            }
        }

        public List<PathNode>? FindPath(Vector2Int gridPositionStart, Vector2Int gridPositionEnd)
        {
            return FindPath(gridPositionStart.X, gridPositionStart.Y, gridPositionEnd.X, gridPositionEnd.Y);
        }

        public List<PathNode>? FindPath(int startX, int startY, int endX, int endY)
        {
            try
            {
                var pathName = $"{startX},{startY}-{endX},{endY}";
                if (_pathCache.TryGetValue(pathName, out var cachedPath))
                    return cachedPath;
                
                var startNode = _pathNodes[startX, startY];
                var endNode = _pathNodes[endX, endY];

                _openList = [startNode];
                _closedList = [];

                for (var x = 0; x < _pathGridWidth; x++)
                {
                    for (var y = 0; y < _pathGridHeight; y++)
                    {
                        var pathNode = _pathNodes[x, y];
                        pathNode.GCost = int.MaxValue;
                        pathNode.CalculateFCost();
                        pathNode.PreviousNodeGridPosition = null;
                    }
                }

                startNode.GCost = 0;
                startNode.HCost = CalculateDistanceCost(startNode, endNode);
                startNode.CalculateFCost();

                while (_openList.Count > 0)
                {
                    var currentNode = GetLowestFCostNode(_openList);
                    if (currentNode == null) break;

                    if (currentNode == endNode)
                    {
                        //destination reached!
                        return CalculatePath(endNode);
                    }

                    _openList.Remove(currentNode);
                    _closedList.Add(currentNode);

                    foreach (var neighborNodePosition in currentNode.Neighbors.AsValueEnumerable().Where(p => p.IsTraversable))
                    {
                        var neighborNode = _pathNodes[neighborNodePosition.GridPosition.X, neighborNodePosition.GridPosition.Y];
                        if (_closedList.Contains(neighborNode))
                            continue;

                        var tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighborNode);
                        if (tentativeGCost < neighborNode.GCost)
                        {
                            neighborNode.PreviousNodeGridPosition = currentNode.GridPosition;
                            neighborNode.GCost = tentativeGCost;
                            neighborNode.HCost = CalculateDistanceCost(neighborNode, endNode);
                            neighborNode.CalculateFCost();

                            if (!_openList.Contains(neighborNode))
                                _openList.Add(neighborNode);
                        }
                    }
                }
            }
            catch
            {
                Log.Error($"Pathfinder Error: {startX},{startY} - {endX},{endY}");
                Log.Info(_pathNodes[startX, startY].ToString());
                Log.Info(_pathNodes[endX, endY].ToString());
                throw;
            }

            // no path found... bummer!
            return null;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode> { endNode };
            var currentNode = endNode;
            while (currentNode.PreviousNodeGridPosition != null)
            {
                var previousNode = _pathNodes[currentNode.PreviousNodeGridPosition.Value.X, currentNode.PreviousNodeGridPosition.Value.Y];
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
                ? lowerMovementCostNodes.OrderBy(p => p.FCost).First()
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

        public PathNode GetNodeAtCoordinate(Vector2Int gridPosition)
        {
            return _pathNodes[gridPosition.X, gridPosition.Y];
        }

        public void UpdatePathNodeNeighbors(Vector2Int gridPosition, List<PathingNeighbor> pathingNeighbors)
        {
            var node = GetNodeAtCoordinate(gridPosition);
            node.Neighbors = pathingNeighbors;
        }
    }
}