using System.Threading.Tasks;
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

    public async void PutUnderDoor()
    {
        GetComponent<Animator>().SetTrigger("ShowFloppy");
        await Task.Delay(1000);
        InputHint.Instance.Show(InputHint.Instance.ThoughtBubble, "Something was slid underneath the door...");
        await Task.Delay(8000);
        InputHint.Instance.Hide();
    }

    public void OnInteract()
    {
        SaveManager.Data.IsFloppyCollected = true;
        SaveManager.SaveData();
        VFX.ItemPickup(transform.position);
        gameObject.SetActive(false);
    }
}
