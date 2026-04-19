using UnityEngine;

public class ClassicLookSetter : MonoBehaviour
{
    [SerializeField] GameObject[] _hovers;
    private void Awake()
    {
    }
    private void Start()
    {
        Settings.Instance.SettingsUpdated += SetClassicLook;
        SetClassicLook();
    }

    public void SetClassicLook()
    {
        if (Settings.ClassicLook)
        {
            foreach (GameObject go in _hovers) { go.SetActive(true); }
        }
        else
        {
            foreach (GameObject go in _hovers) { go.SetActive(false); }
        }
    }
}
