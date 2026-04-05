using JetBrains.Annotations;
using UnityEngine;

public class Safe : Puzzle
{
    public static Safe Instance;
    [SerializeField] private GameObject _safeCanvas;
    [SerializeField] private GameObject[] _digitHighlights;
    [SerializeField] private SafeDigit[] _digits;
    private int _selectedDigitIndex;
    public int CorrectPin;
    public int PinValue { 
        get
        {
            string pin = $"{_digits[0].Value}{_digits[1].Value}{_digits[2].Value}{_digits[3].Value}";
            Debug.Log(pin);
            return int.Parse(pin);
        } 
    }

    private void Awake()
    {
        Instance = this;
    }
    public new void EnablePuzzleScreen()
    {
        base.EnablePuzzleScreen();
        _safeCanvas.SetActive(true);
        Right(); Left();

        if (Settings.IsUsingGamepad) InputHint.Instance.Show(InputHint.Instance.GamepadEast, "Back");
        else InputHint.Instance.Show(InputHint.Instance.KeyboardEsc, "Back");
    }
    public new void DisablePuzzleScreen()
    {
        base.DisablePuzzleScreen();
        _safeCanvas.SetActive(false);
        InputHint.Instance.Hide();
    }
    public override void Up()
    {
        _digits[_selectedDigitIndex].Decrement();
    }
    public override void Down()
    {
        _digits[_selectedDigitIndex].Increment();
    }
    public override void Left()
    {
        _selectedDigitIndex -= 1;
        if (_selectedDigitIndex < 0) _selectedDigitIndex = _digitHighlights.Length - 1;
        foreach (GameObject highlight in _digitHighlights) highlight.SetActive(false);
        _digitHighlights[_selectedDigitIndex].SetActive(true);
        //Debug.Log(PinValue);
    }
    public override void Right()
    {
        _selectedDigitIndex = (_selectedDigitIndex + 1) % (_digitHighlights.Length);
        foreach (GameObject highlight in _digitHighlights) highlight.SetActive(false);
        _digitHighlights[_selectedDigitIndex].SetActive(true);
    }
    public override void Back()
    {
        DisablePuzzleScreen();
    }
    public override void PuzzleProcess()
    {
        
    }
}
