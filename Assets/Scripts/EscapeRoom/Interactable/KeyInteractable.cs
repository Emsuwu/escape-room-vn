using UnityEngine;

public class KeyInteractable : Interactable
{
    public void OnKeyInteract()
    {
        if (SaveManager.HasFlag("AdventureComplete"))
        {
            SaveManager.AddFlag("KeyCollected");
            SaveManager.SaveData();
            gameObject.SetActive(false);
        }
    }
}
