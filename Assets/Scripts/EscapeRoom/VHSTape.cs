using UnityEngine;

public class VHSTape : MonoBehaviour
{
    public void OnInteract()
    {
        SaveManager.Data.IsTapeCollected = true;
        SaveManager.SaveData();
        VFX.ItemPickup(transform.position);
        if (SaveManager.Data.IsTapeCollected) gameObject.SetActive(false);
    }
    private void Start()
    {
        if (SaveManager.Data.IsTapeCollected) gameObject.SetActive(false);
    }
}
