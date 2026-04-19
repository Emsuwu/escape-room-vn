using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogChoiceButton : MonoBehaviour
{
    public static int ChoicesOnScreen = 0;
    public int ChoiceNumber = 0;
    public DialogCommand Command;
    [SerializeField] private TMPro.TMP_Text _text;
    public string Text {  get { return _text.text; } set { _text.text = value; } }
    private void OnEnable()
    {
        ChoicesOnScreen += 1;
        ChoiceNumber = ChoicesOnScreen - 1;

        if (ChoiceNumber == 0) { GetComponent<Button>().Select(); }
    }
    public void Trigger()
    {
        ChoicesOnScreen = 0;
        Command.Execute();
        Dialog.Instance.ClearChoicesAndContinue();
    }
}
