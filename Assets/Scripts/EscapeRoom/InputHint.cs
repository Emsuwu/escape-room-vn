using UnityEngine;
using UnityEngine.UI;

public class InputHint : MonoBehaviour
{
    public static InputHint Instance { get; private set; }

    [Header("Gamepad Icons")]
    public Sprite GamepadNorth;
    public Sprite GamepadSouth;
    public Sprite GamepadEast;
    public Sprite GamepadWest;
    [Header("Keyboard and Mouse Icons")]
    public Sprite KeyboardEsc;

    [SerializeField] private Image _hintIcon;
    [SerializeField] private TMPro.TMP_Text _hintText;
    [SerializeField] private Image _line;
    private Image _backgroundImage;

    private void Awake()
    {
        Instance = this;
    }
    public void SetText(string text)
    {
        _hintText.text = text;
    }
    public void SetIcon(Sprite icon)
    {
        _hintIcon.sprite = icon;
    }

    private void Start()
    {
        _backgroundImage = GetComponent<Image>();
        Hide(0f);
    }

    public void Show(Sprite icon, string text, float duration = 0.5f)
    {
        SetIcon(icon); SetText(text);
        _backgroundImage.CrossFadeAlpha(1f, duration, false);
        _hintIcon.CrossFadeAlpha(1f, duration, false);
        _hintText.CrossFadeAlpha(1f, duration, false);
        _line.CrossFadeAlpha(1f, duration, false);
    }
    public void Hide(float duration = 0.5f)
    {
        _backgroundImage.CrossFadeAlpha(0f, duration, false);
        _hintIcon.CrossFadeAlpha(0f, duration, false);
        _hintText.CrossFadeAlpha(0f, duration, false);
        _line.CrossFadeAlpha(0f, duration, false);
    }
}
