using System.Collections;
using UnityEngine;

public class ClockPuzzle : Puzzle
{
    [SerializeField] private GameObject _interactableGo;
    [SerializeField] private GameObject _outlineGo;
    private Coroutine _coCycleHints;

    [SerializeField] private GameObject _minuteHand;
    [SerializeField] private GameObject _hourHand;

    private int _minutes;
    private int _hourHandNumber;
    private int _minuteHandNumber;

    public string Time { get
        {
            int hours = (int)Mathf.Floor(_minutes / 60) % 12;
            int minutes = _minutes % 60;
            if (hours == 0) hours = 12;
            string time = $"{hours}:{minutes}";
            return time;
        }
    }

    private void MoveHands(int minutes)
    {
        _minutes += minutes;
        if (_minutes < 0) _minutes = 720 + _minutes;

        _minuteHand.transform.Rotate(0, minutes * -6, 0, Space.Self);
        _hourHand.transform.Rotate(0, minutes * -0.5f, 0, Space.Self);

        Debug.Log(Time + " " + _minutes);
    }
    public override void Right() { MoveHands(5); }
    public override void Left() { MoveHands(-5); }
    public override void Up() { MoveHands(30); }
    public override void Down() { MoveHands(-30); }

    public new void Enter()
    {
        base.Enter();
        _interactableGo.SetActive(false);
        _outlineGo.SetActive(false);
        Debug.Log("nyun!");
        _coCycleHints = StartCoroutine(CoCycleHints());
    }
    public new void Exit() 
    { 
        base.Exit(); 
        _interactableGo.SetActive(true);
        _outlineGo.SetActive(true);
        if (_coCycleHints != null) StopCoroutine(_coCycleHints);
        InputHint.Instance.Hide();
    }

    public override void Back()
    {
        Exit();
    }

    private IEnumerator CoCycleHints()
    {
        Debug.Log("A!");
        Sprite hintImage;
        string hintText;
        const float Delay = 6f;
        Sprite[] gamepadIcons = {
                InputHint.Instance.GamepadDpadRight,
                InputHint.Instance.GamepadDpadLeft,
                InputHint.Instance.GamepadDpadUp,
                InputHint.Instance.GamepadDpadDown,
                InputHint.Instance.GamepadEast,
            };
        Sprite[] keyboardIcons = {
                InputHint.Instance.KeyboardRight,
                InputHint.Instance.KeyboardLeft,
                InputHint.Instance.KeyboardUp,
                InputHint.Instance.KeyboardDown,
                InputHint.Instance.KeyboardEsc,
            };
        string[] texts = {
                "Add 5 minutes",
                "Take 5 minutes",
                "Add 30 minutes",
                "Take 30 minutes",
                "Exit"
            };
        while (true)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (Settings.IsUsingGamepad) hintImage = gamepadIcons[i];
                else hintImage = keyboardIcons[i];
                hintText = texts[i];

                InputHint.Instance.Show(hintImage, hintText);
                yield return new WaitForSeconds(Delay);
                InputHint.Instance.Hide();
                yield return new WaitForSeconds(0.55f);
            }
        }
    }
}
