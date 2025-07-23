using Engine.Logging;
using Engine.Systems;
using Raylib_cs;

namespace Engine.Components2d;

public class Song : Component2d
{
    private float _localVolume = 1f;
    private float _localPitch = 1f;
    
    public bool Loop { get; }
    public Music Music { get; }
    public float Volume { get; private set; }
    public float Pitch { get; private set; }

    public Song(Music music, float volume = 1f, float pitch = 1f, bool loop = true)
    {
        Music = music;
        _localVolume = Math.Clamp(volume, 0f, 1f);
        _localPitch = Math.Clamp(pitch, 0f, 1f);
        Volume = AudioMasterSystem.Volume * _localVolume;
        Pitch = AudioMasterSystem.Pitch * _localPitch;
        Loop = loop;
        Raylib.SetMusicVolume(Music, Volume);
        Raylib.SetMusicPitch(Music, Pitch);

        AudioMasterSystem.VolumeChanged += HandleMasterVolumeChanged;
        AudioMasterSystem.PitchChanged += HandleMasterPitchChanged;
    }

    private void HandleMasterVolumeChanged(float newMasterVolume)
    {
        Volume = newMasterVolume * _localVolume;
        Raylib.SetMusicVolume(Music, Volume);
    }
    
    private void HandleMasterPitchChanged(float newMasterPitch)
    {
        Pitch = newMasterPitch * _localPitch;
        Raylib.SetMusicPitch(Music, Pitch);
    }

    public void Play()
    {
        Raylib.PlayMusicStream(Music);
        EngineManager.CurrentMusic = Music;
    }
}