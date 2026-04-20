using System.Linq;
using UnityEngine;
public class SwitchMenus : MonoBehaviour
{
    [SerializeField] GameObject[] _setActive;
    [SerializeField] GameObject[] _setInactive;
    public void OnClick()
    {
        if (_setActive != null && _setActive.Length > 0) foreach (GameObject go in _setActive) { go.SetActive(true); }
        if (_setInactive != null && _setInactive.Length > 0) foreach (GameObject go in _setInactive) { go.SetActive(false); }
    }
}
