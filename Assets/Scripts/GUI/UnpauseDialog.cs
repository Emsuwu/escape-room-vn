using UnityEngine;

public class UnpauseDialog : MonoBehaviour
{
    public void OnClick()
    {
        DialogPauseHandler.Instance.IsPaused = false;

        Debug.Log("BWA!");
    }
}
