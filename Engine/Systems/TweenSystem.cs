namespace Engine.Systems;

public static class TweenSystem
{
    internal static bool LoggingEnabled;
    public static void EnableLogging() => LoggingEnabled = true;
    
    private static readonly List<Tween> Tweens = [];
    
    internal static void StartTween(Tween tween) => Tweens.Add(tween);

    internal static void Update(float delta)
    {
        for (var i = Tweens.Count - 1; i >= 0; i--)
        {
            var tween = Tweens[i];

            if (tween.Finished)
            {
                Tweens.Remove(tween);
                continue;
            }
            
            tween.Update(delta);
        }
    }
}