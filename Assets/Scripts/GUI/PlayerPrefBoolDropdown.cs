using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrefBoolDropdown : MonoBehaviour
{
    [SerializeField] string _targetPref;
    [SerializeField] bool _zeroIsOff = true;
    private TMP_Dropdown _dropdown;
    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }
    private void OnEnable()
    {
        if (_zeroIsOff) _dropdown.value = PlayerPrefs.GetInt(_targetPref);
        else _dropdown.value = PlayerPrefs.GetInt(_targetPref) == 0 ? 1 : 0;
    }
    public void OnValueChanged()
    {
        int v = _dropdown.value;
        if (v != 0 && v != 1)
        {
            Debug.LogError("Can only have 0 and 1 as elements in bool dropdown");
            return;
        }
        if (!_zeroIsOff) v = v == 0 ? 1 : 0;
        
        PlayerPrefs.SetInt(_targetPref, v);
        Settings.UpdateSettings();
    }
}
