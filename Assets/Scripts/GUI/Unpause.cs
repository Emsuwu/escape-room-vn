using UnityEngine;

public class Unpause : MonoBehaviour
{
    public void OnClick()
    {
        Meowtils.Defer(() => PlayerInputHandler.Instance.IsPaused = false);
    }
}
