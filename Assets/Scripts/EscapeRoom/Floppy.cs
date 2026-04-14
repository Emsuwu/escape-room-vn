using UnityEngine;

public class Floppy : MonoBehaviour
{
    void Start()
    {
        if (SaveManager.Data.IsClockPuzzleSolved)
        {
            PutUnderDoor();
        }
    }

    public void PutUnderDoor()
    {
        GetComponent<Animator>().SetTrigger("ShowFloppy");
    }
}
