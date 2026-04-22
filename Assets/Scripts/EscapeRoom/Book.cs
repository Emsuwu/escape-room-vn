using UnityEngine;

public class Book : MonoBehaviour
{
    public void OnInteract()
    {
        Dialog.StartDialogScene("Unicorn/1", "start");
    }
}
