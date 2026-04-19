using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefSlider : MonoBehaviour
{
    [SerializeField] private string _targetPref;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        _slider.value = PlayerPrefs.GetFloat(_targetPref);
    }
    public void OnValueChanged()
    {
        PlayerPrefs.SetFloat(_targetPref, _slider.value);
        Settings.UpdateSettings();
        Debug.Log("MEOW " + PlayerPrefs.GetFloat(_targetPref) + " " + Settings.Instance.MasterVolume);
    }
}
