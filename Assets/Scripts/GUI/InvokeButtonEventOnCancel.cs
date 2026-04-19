using UnityEngine;
using UnityEngine.UI;

public class InvokeButtonEventOnCancel : MonoBehaviour
{
    private void OnEnable()
    {
        UIInputHandler.Instance.OnCancel += OnCancel;
    }
    private void OnDisable()
    {
        UIInputHandler.Instance.OnCancel -= OnCancel;
    }
    public void OnCancel()
    {
        if (!gameObject.activeSelf) return;
        GetComponent<Button>().onClick?.Invoke();
    }
}
