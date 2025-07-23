using Engine.Systems;
using Raylib_cs;

namespace Engine.Components2d;

public class SoundEffect : Component2d
{
    private float _localVolume = 1f;
    private float _localPitch = 1f;
    
    public Sound Sound { get; }
    public float Volume { get; private set; }
    public float Pitch { get; private set; }

    public SoundEffect(Sound sound, float volume = 1f, float pitch = 1f)
    {
        Sound = sound;
        _localVolume = Math.Clamp(volume, 0f, 1f);
        _localPitch = Math.Clamp(pitch, 0f, 1f);
        Volume = AudioMasterSystem.Volume * _localVolume;
        Pitch = AudioMasterSystem.Pitch * _localPitch;
        Raylib.SetSoundVolume(Sound, Volume);
        Raylib.SetSoundPitch(Sound, Pitch);
        
        AudioMasterSystem.VolumeChanged += HandleMasterVolumeChanged;
        AudioMasterSystem.PitchChanged += HandleMasterPitchChanged;
    }
    
    private void HandleMasterVolumeChanged(float newMasterVolume)
    {
        Volume = newMasterVolume * _localVolume;
        Raylib.SetSoundVolume(Sound, Volume);
    }
    
    private void HandleMasterPitchChanged(float newMasterPitch)
    {
        Pitch = newMasterPitch * _localPitch;
        Raylib.SetSoundPitch(Sound, Pitch);
    }
    
    public void Play() => Raylib.PlaySound(Sound);
}