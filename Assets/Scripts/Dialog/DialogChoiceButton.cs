using UnityEngine;

public class DialogChoiceButton : MonoBehaviour
{
    public DialogCommand Command;
    [SerializeField] private TMPro.TMP_Text _text;
    public string Text {  get { return _text.text; } set { _text.text = value; } }
    public void Trigger()
    {
        Command.Execute();
        Dialog.Instance.ClearChoicesAndContinue();
    }
}
