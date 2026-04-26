using UnityEngine;

public class Book : MonoBehaviour
{
    public void OnInteract()
    {
        SceneFadeImage.Instance.FadeToBlackThenExecute(() => Dialog.StartDialogScene("Unicorn/1", "start"), 2f, 2.25f);

    }
}
