namespace Engine.Extensions;

public static class CollectionExtensions
{
    public static T PickRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }
}