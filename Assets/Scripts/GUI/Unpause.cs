using UnityEngine;

public class Unpause : MonoBehaviour
{
    public void OnClick()
    {
        PlayerInputHandler.Instance.IsPaused = false;
    }
}
