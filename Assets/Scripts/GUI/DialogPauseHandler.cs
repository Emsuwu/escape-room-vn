using UnityEngine;

public class DialogPauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private GameObject _settingsMenu;
    public bool IsPaused;
    public static DialogPauseHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIInputHandler.Instance.OnPause += OnPause;
    }

    //private void Update()
    //{
    //    Debug.Log("Paused? " + DialogPauseHandler.Instance.IsPaused);
    //}

    public void OnPause()
    {
        if (!IsPaused)
        {
            IsPaused = true;
            _menuRoot.SetActive(true);
            _settingsMenu.SetActive(true);
        }
        else
        {
            IsPaused = false;
            _menuRoot.SetActive(false);
            _settingsMenu.SetActive(false);
        }
    }
}
