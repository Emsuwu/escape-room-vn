using UnityEngine;

public class Floppy : MonoBehaviour
{
    void Start()
    {
        if (SaveManager.Data.IsFloppyCollected) { 
            gameObject.SetActive(false);
            return;
        }
        if (!SaveManager.Data.IsClockPuzzleSolved) return;
        PutUnderDoor();
    }
    public void PutUnderDoor()
    {
        GetComponent<Animator>().SetTrigger("ShowFloppy");
    }
    public void OnInteract()
    {
        SaveManager.Data.IsFloppyCollected = true;
        SaveManager.SaveData();
        gameObject.SetActive(false);
    }
}
