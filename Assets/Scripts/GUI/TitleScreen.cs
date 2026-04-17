using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private GameObject _settings;
    private void Start()
    {
        Settings.UpdateSettings();
    }
    public void ContinuePressed()
    {
        SceneManager.LoadScene(1);        
    }
    public void NewGamePressed()
    {
        SaveManager.ClearData();
        SceneManager.LoadScene(1);
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
    public void SettingsPressed()
    {
        _menuRoot.SetActive(true);
        _settings.SetActive(true);
        gameObject.SetActive(false);
    }
}
