using TMPro;
using UnityEngine;

public class InvertYDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        bool invertCamY = PlayerPrefs.GetInt("InvertCamY") == 1 ? true : false;
        bool invertReticleY = PlayerPrefs.GetInt("InvertReticleY") == 1 ? true : false;

        if (!invertCamY && !invertReticleY) { _dropdown.value = 0; }
        if (invertCamY && !invertReticleY) { _dropdown.value = 1; }
        if (!invertCamY && invertReticleY) { _dropdown.value = 2; }
        if (invertCamY && invertReticleY) { _dropdown.value = 3; }
    }

    public void OnValueChanged()
    { 
        Debug.Log(_dropdown.value);

        switch (_dropdown.value)
        {
            // Don't invert Y
            case 0:
                PlayerPrefs.SetInt("InvertCamY", 0);
                PlayerPrefs.SetInt("InvertReticleY", 0);
                break;
            // Invert camera Y
            case 1:
                PlayerPrefs.SetInt("InvertCamY", 1);
                PlayerPrefs.SetInt("InvertReticleY", 0);
                break;
            // Invert reticle Y
            case 2:
                PlayerPrefs.SetInt("InvertReticleY", 1);
                PlayerPrefs.SetInt("InvertCamY", 0);
                break;
            // Invert both
            case 3:
                PlayerPrefs.SetInt("InvertReticleY", 1);
                PlayerPrefs.SetInt("InvertCamY", 1);
                break;
        }

        Settings.UpdateSettings();
    }
}
