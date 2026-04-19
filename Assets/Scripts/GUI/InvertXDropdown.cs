using TMPro;
using UnityEngine;

public class InvertXDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        bool invertCamX = PlayerPrefs.GetInt("InvertCamX") == 1 ? true : false;
        bool invertReticleX = PlayerPrefs.GetInt("InvertReticleX") == 1 ? true : false;

        if (!invertCamX && !invertReticleX) { _dropdown.value = 0; }
        if (invertCamX && !invertReticleX) { _dropdown.value = 1; }
        if (!invertCamX && invertReticleX) { _dropdown.value = 2; }
        if (invertCamX && invertReticleX) { _dropdown.value = 3; }
    }

    public void OnValueChanged()
    {
        Debug.Log(_dropdown.value);

        switch (_dropdown.value)
        {
            // Don't invert X
            case 0:
                PlayerPrefs.SetInt("InvertCamX", 0);
                PlayerPrefs.SetInt("InvertReticleX", 0);
                break;
            // Invert camera X
            case 1:
                PlayerPrefs.SetInt("InvertCamX", 1);
                PlayerPrefs.SetInt("InvertReticleX", 0);
                break;
            // Invert reticle X
            case 2:
                PlayerPrefs.SetInt("InvertReticleX", 1);
                PlayerPrefs.SetInt("InvertCamX", 0);
                break;
            // Invert both
            case 3:
                PlayerPrefs.SetInt("InvertReticleX", 1);
                PlayerPrefs.SetInt("InvertCamX", 1);
                break;
        }

        Settings.UpdateSettings();
    }
}
