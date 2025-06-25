using Raylib_cs;

namespace Engine.Components2d;

public class SoundEffect : Component2d
{
    public Sound Sound { get; }
    public float Volume { get; }
    public float Pitch { get; }

    public SoundEffect(Sound sound, float volume = 1f, float pitch = 1f)
    {
        Sound = sound;
        Volume = volume;
        Pitch = pitch;
        Raylib.SetSoundVolume(Sound, Volume);
        Raylib.SetSoundPitch(Sound, Pitch);
    }
    
    public void Play() => Raylib.PlaySound(Sound);
}