using UnityEngine;

public class KeyInteractable : Interactable
{
    private new void Start()
    {
        base.Start();
        if (SaveManager.HasFlag("KeyCollected")) { gameObject.SetActive(false); }
    }
    public void OnKeyInteract()
    { 
        if (SaveManager.HasFlag("AdventureComplete"))
        {
            SaveManager.AddFlag("KeyCollected");
            SaveManager.SaveData();
            VisualEffect.ItemPickup(transform.position);
            gameObject.SetActive(false);
        }
    }
}
