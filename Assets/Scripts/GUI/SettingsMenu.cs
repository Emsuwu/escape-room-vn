using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void BackToTitle() {
        SceneManager.LoadScene(0);
    }
    public void BackToEscapeRoom()
    {
        SceneManager.LoadScene(1);
    }
}
