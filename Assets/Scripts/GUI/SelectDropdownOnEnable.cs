using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SelectDropdownOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TMP_Dropdown>().Select();
    }
}
