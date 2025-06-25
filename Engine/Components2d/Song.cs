using Raylib_cs;

namespace Engine.Components2d;

public class Song : Component2d
{
    public bool Loop { get; }
    public Music Music { get; }
    public float Volume { get; }
    public float Pitch { get; }

    public Song(Music music, float volume = 1f, float pitch = 1f, bool loop = true)
    {
        Music = music;
        Volume = volume;
        Pitch = pitch;
        Loop = loop;
        Raylib.SetMusicVolume(Music, Volume);
        Raylib.SetMusicPitch(Music, Pitch);
    }
    
    public void Play()
    {
        Raylib.PlayMusicStream(Music);
        EngineManager.CurrentMusic = Music;
    }
}