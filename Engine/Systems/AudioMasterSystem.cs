namespace Engine.Systems;

public class AudioMasterSystem
{
    public static Action<float> VolumeChanged;
    public static Action<float> PitchChanged;
    
    private static float _volume = 1f;
    private static float _pitch = 1f;

    public static float Volume
    {
        get => _volume;
        set
        {
            _volume = Math.Clamp(value, 0f, 1f);
            VolumeChanged.Invoke(_volume);
        }
    }

    public static float Pitch
    {
        get => _pitch;
        set
        {
            _pitch = value;
            PitchChanged.Invoke(_pitch);
        }
    }
}