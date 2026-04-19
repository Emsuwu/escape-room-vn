using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _masterVolume;
    [SerializeField] private AudioMixerGroup _soundEffectsVolume;
    [SerializeField] private AudioMixerGroup _ambienceVolume;
    [SerializeField] private AudioMixerGroup _musicVolume;

    public static Settings Instance;
    public static bool IsUsingGamepad;
    public static float CameraSensitivity;
    public static float ReticleSensitivity;
    public static bool InvertCamY;
    public static bool InvertCamX;
    public static bool InvertReticleY;
    public static bool InvertReticleX;
    public static void UpdateSettings()
    {
        CameraSensitivity = UpdateKey("CameraSensitivity", 25f);
        ReticleSensitivity = UpdateKey("ReticleSensitivity", 2f);
        InvertCamY = UpdateKey("InvertCamY", false);
        InvertCamX = UpdateKey("InvertCamX", false);
        InvertReticleY = UpdateKey("InvertReticleY", false);
        InvertReticleX = UpdateKey("InvertReticleX", false);
        Instance._masterVolume.audioMixer.SetFloat("MasterVolume", UpdateKey("MasterVolume", 0));
        Instance._soundEffectsVolume.audioMixer.SetFloat("SoundEffectsVolume", UpdateKey("SoundEffectsVolume", 0));
        Instance._musicVolume.audioMixer.SetFloat("MusicVolume", UpdateKey("MusicVolume", 0));
        Instance._ambienceVolume.audioMixer.SetFloat("AmbienceVolume", UpdateKey("AmbienceVolume", 0));
    }
    private void Awake()
    {
        Instance = this;
    }
    private static int UpdateKey(string key, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        else return defaultValue;
    }
    private static bool UpdateKey(string key, bool defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key) == 1 ? true : false;
        else return defaultValue;
    }
    private static float UpdateKey(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetFloat(key);
        else return defaultValue;
    }
}
