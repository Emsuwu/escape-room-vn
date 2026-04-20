using UnityEngine;

public class DialogPauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private GameObject _settingsMenu;

    private void Start()
    {
        UIInputHandler.Instance.OnPause += OnPause;
    }

    public void OnPause()
    {
        Debug.Log("meow");
        _menuRoot.SetActive(true);
        _settingsMenu.SetActive(true);
        Dialog.Instance.PauseExecution();
    }
}
