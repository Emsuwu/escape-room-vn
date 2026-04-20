using UnityEngine;

public class UnpauseDialog : MonoBehaviour
{
    public void OnClick()
    {
        Meowtils.Defer( () => Dialog.Instance.PauseExecution(false) );
    }
}
