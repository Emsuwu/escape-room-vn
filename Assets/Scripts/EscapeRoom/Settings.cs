using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float CameraSensitivity;
    public static float ReticleSensitivity;
    public static void UpdateSettings()
    {
        PlayerPrefs.DeleteAll();
        CameraSensitivity = UpdateKey("CameraSensitivity", 25f);
        ReticleSensitivity = UpdateKey("ReticleSensitivity", 3f);
    }
    private static float UpdateKey(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey("CameraSensitivity")) return PlayerPrefs.GetFloat("CameraSensitivity");
        else return defaultValue;
    }
}
