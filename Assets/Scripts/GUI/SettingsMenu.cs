using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private GameObject _titleScreen;
    public void CloseSettingsMenu()
    {
        _menuRoot.SetActive(false);
        gameObject.SetActive(false);
        _titleScreen.SetActive(true);
    }
}
