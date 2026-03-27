using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _characterNameText;

    public static DialogBox Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCharacterName(string name)
    {
        _characterNameText.text = name;
    }

    private void OnEnable()
    {
        SetCharacterName("Hi");
    }
}
