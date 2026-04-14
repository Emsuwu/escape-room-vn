using NaughtyAttributes;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour  
{
    public static string SavePath { get
        {
            return Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Save.json";
        } 
    }
    public static PersistentData Data;

    private void Awake()
    {
        if (Data == null) Data = new PersistentData();
        // Creates a save file if there isn't one
        try { LoadData(); } catch { SaveData(); }
    }

    [Button("Clear save data")]
    public static void ClearData()
    {
        Data = new PersistentData();
        SaveData();
    }
    public static void SaveData()
    {
        string json = JsonUtility.ToJson(Data);
        using (StreamWriter writer = new StreamWriter(SavePath)) 
        { 
            writer.Write(json);
        }
        Debug.Log($"Data saved to { SavePath }");
    }
    public static void LoadData() 
    { 
        string json = string.Empty;

        using (StreamReader reader = new StreamReader(SavePath))
        {
            json = reader.ReadToEnd();
        }
        PersistentData data = JsonUtility.FromJson<PersistentData>(json);
        Data = data;
        Debug.Log($"Data loaded from {SavePath}");
    }
}

public class PersistentData
{
    public bool IsSafeCracked = false;
    public bool IsTapeCollected = false;
    public bool IsClockPuzzleSolved = false;
}
