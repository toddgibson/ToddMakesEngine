using Engine.Utilities;
using ZLinq;

namespace Engine.Systems;

public static class GridSystem
{
    private static readonly List<Grid> Grids = [];

    internal static void DrawInternal()
    {
        foreach (var grid in Grids.AsValueEnumerable().Where(p => p.DrawGridLines))
        {
            grid.DrawInternal();
        }
    }

    internal static void Add(Grid grid)
    {
        Grids.Add(grid);
    }

    internal static void Remove(Grid grid)
    {
        Grids.Remove(grid);
    }
}