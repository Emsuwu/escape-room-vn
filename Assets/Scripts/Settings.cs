using UnityEngine;

public class Settings : MonoBehaviour
{
    public static bool IsUsingGamepad;
    public static float CameraSensitivity;
    public static float ReticleSensitivity;
    public static bool InvertY;
    public static bool InvertX;
    public static void UpdateSettings()
    {
        CameraSensitivity = UpdateKey("CameraSensitivity", 25f);
        ReticleSensitivity = UpdateKey("ReticleSensitivity", 2f);
        InvertY = UpdateKey("InvertY", false);
        InvertX = UpdateKey("InvertX", false);
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
