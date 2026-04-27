using UnityEngine;

public class DebugUnlock : MonoBehaviour
{
    private void Start()
    {
        SaveManager.AddFlag("AdventureComplete");
        SaveManager.AddFlag("UnicornComplete");
        SaveManager.AddFlag("UrbexComplete");
        SaveManager.SaveData();
    }
}
