using UnityEngine;

public class Book : MonoBehaviour
{
    public void OnInteract()
    {
        Dialog.StartDialogScene("Book", "start");
    }
}
