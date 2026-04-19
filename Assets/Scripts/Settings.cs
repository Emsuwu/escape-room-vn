using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public float MasterVolume
    {
        get
        {
            Instance._masterMixer.GetFloat("MasterVolume", out float mv);
            return mv;
        }
    }
    [SerializeField] private AudioMixer _masterMixer;

    public static Settings Instance;
    public static bool IsUsingGamepad;
    public static float CameraSensitivity;
    public static float ReticleSensitivity;
    public static bool InvertCamY;
    public static bool InvertCamX;
    public static bool InvertReticleY;
    public static bool InvertReticleX;
    public static bool ClassicLook;

    public delegate void OnSettingsUpdated();
    public event OnSettingsUpdated SettingsUpdated;
    public static void UpdateSettings()
    {
        CameraSensitivity = UpdateKey("CameraSensitivity", 25f);
        ReticleSensitivity = UpdateKey("ReticleSensitivity", 2f);
        InvertCamY = UpdateKey("InvertCamY", false);
        InvertCamX = UpdateKey("InvertCamX", false);
        InvertReticleY = UpdateKey("InvertReticleY", false);
        InvertReticleX = UpdateKey("InvertReticleX", false);
        ClassicLook = UpdateKey("ClassicLook", false);
        Instance._masterMixer.SetFloat("MasterVolume", UpdateKey("MasterVolume", -5f));
        Instance._masterMixer.SetFloat("SoundEffectsVolume", UpdateKey("SoundEffectsVolume", 0f));
        Instance._masterMixer.SetFloat("MusicVolume", UpdateKey("MusicVolume", 0f));
        Instance._masterMixer.SetFloat("AmbienceVolume", UpdateKey("AmbienceVolume", 0f));
        Instance?.SettingsUpdated?.Invoke();
    }
    private void Awake()
    {
        Instance = this;
        UpdateSettings();
    }
    private static int UpdateKey(string key, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        else
        {
            PlayerPrefs.SetInt(key, defaultValue);
            return defaultValue;
        }
    }
    private static bool UpdateKey(string key, bool defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key) == 1 ? true : false;
        else
        {
            PlayerPrefs.SetInt(key, defaultValue ? 1 : 0);
            return defaultValue;
        }
    }
    private static float UpdateKey(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetFloat(key);
        else
        {
            PlayerPrefs.SetFloat(key, defaultValue);
            return defaultValue;
        }
    }
}
