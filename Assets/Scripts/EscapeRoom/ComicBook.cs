using UnityEngine;

public class ComicBook : MonoBehaviour
{
    public void OnInteract()
    {
        Dialog.StartDialogScene("Comic", "start");
    }
    private void Start()
    {
        if (SaveManager.Data.IsComicSlidUnderDoor) { SlideUnderDoor(); }
    }
    public void SlideUnderDoor()
    {
        GetComponent<Animator>().SetTrigger("Slide");
        SaveManager.Data.IsComicSlidUnderDoor = true;
        SaveManager.SaveData();
    }
}
