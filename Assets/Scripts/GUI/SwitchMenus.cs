using UnityEngine;
public class SwitchMenus : MonoBehaviour
{
    [SerializeField] GameObject[] _setActive;
    [SerializeField] GameObject[] _setInactive;
    public void OnClick()
    {
        foreach (GameObject go in _setActive) { go.SetActive(true); }
        foreach (GameObject go in _setInactive) { go.SetActive(false); }
    }
}
