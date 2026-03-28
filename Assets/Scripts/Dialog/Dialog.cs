using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TextAsset _file;
    private Coroutine _currentSequenceCoroutine;
    public float CharacterInterval = 0.1f;
    private bool _isPrintingDialog;
    public static Dialog Instance;

    [SerializeField] private TMPro.TMP_Text _dialogText;
    [SerializeField] private TMPro.TMP_Text _characterNameText;

    [SerializeField] InputActionAsset _inputActionAsset;
    InputAction _advanceAction;

    private void Awake()
    {
        Instance = this;
        _advanceAction = InputSystem.actions.FindAction("Advance");
    }
    private void Start()
    {
        if (_file == null) return;
        DialogParser.LoadDialog(_file.text);
        Branch("unicorn_1");
        StartSequence();
    }
    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("Dialog").Enable();
    }
    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("Dialog").Disable();
    }

    /// <summary>
    /// Loads the provided dialog file at specified target sequence
    /// </summary>
    /// <param name="dialogFile"></param>
    /// <param name="targetSequence"></param>
    public void SetDialogFile(TextAsset dialogFile, string targetSequence)
    {
        SetDialogFile(dialogFile);
        DialogParser.Branch(targetSequence);
    }
    /// <summary>
    /// Loads a dialog file into DialogParser
    /// </summary>
    /// <param name="dialogFile"></param>
    public void SetDialogFile(TextAsset dialogFile)
    {
        _file = dialogFile;
        DialogParser.LoadDialog(_file.text);
    }
    /// <summary>
    /// A wrapper for DialogParser.Branch
    /// </summary>
    /// <param name="targetSequenceName"></param>
    public void Branch(string targetSequenceName)
    {
        DialogParser.Branch(targetSequenceName);
    }

    /// <summary>
    /// Starts a dialog file at the target sequence provided
    /// </summary>
    /// <param name="dialogFile">Reference to a .txt file containing dialog</param>
    /// <param name="targetSequenceName">The name of the sequence that the dialog will start with</param>
    public void StartSequenceInFile(TextAsset dialogFile, string targetSequenceName)
    {
        SetDialogFile(dialogFile, targetSequenceName);
        StartSequence();
    }
    /// <summary>
    /// Starts the already loaded dialog file at the target sequence (if there is one)
    /// </summary>
    /// <param name="targetSequenceName"></param>
    public void StartSequence(string targetSequenceName)
    {
        if (_file == null) return;
        DialogParser.Branch(targetSequenceName);
        StartSequence();
    }
    /// <summary>
    /// Starts the current sequence at the loaded dialog file 
    /// </summary>
    public void StartSequence()
    {
        if (_file == null) return;
        if (DialogParser.CurrentDialogSequence == null) return;
        _currentSequenceCoroutine = StartCoroutine(CoDialogSequence());
    }

    private IEnumerator CoDialogSequence()
    {
        while (DialogParser.CurrentDialogSequence.DialogCommands.Count > 0)
        {

            DialogCommand current_command = DialogParser.DequeueCommand();

            if (current_command.PauseAfterExecution)
            {
                if (current_command.Command == "dialog")
                {
                    current_command.Execute();
                    yield return new WaitUntil(() => !_isPrintingDialog);
                }

                yield return null;
            }
            yield return null;

        }
    }

    public void SetCharacterName(string characterName)
    {
        _characterNameText.text = characterName;
    }

    /// <summary>
    /// Prints a dialog string to the dialog box
    /// </summary>
    /// <param name="dialogString"></param>
    public void PrintDialog(string dialogString)
    {
        PrintDialog(dialogString.ConvertTo<string[]>());
    }
    /// <summary>
    /// Prints an array of dialog strings and pauses to the dialog box
    /// </summary>
    /// <param name="dialogArray"></param>
    public void PrintDialog(string[] dialogArray)
    {
        _isPrintingDialog = true;
        StartCoroutine(CoPrintDialog(dialogArray));
    }
    private IEnumerator CoPrintDialog(string[] dialogArray)
    {
        _dialogText.text = string.Empty;
        foreach (string dialog in dialogArray)
        {
            // If it is a wait that is specified, pause for specified time
            //if (dialog.Substring(dialog.Length-2, dialog.Length-1) == "s")
            //{
            //    float num;
            //    bool isNumeric = float.TryParse((dialog.Substring(0, dialog.Length - 2)), out num);
            //    if (isNumeric)
            //    {
            //        yield return new WaitForSeconds(num);
            //        continue;
            //    }
            //}
            // Otherwise add the characters to the dialog text, speeding up if click is held
            foreach (char character in dialog)
            {
                _dialogText.text += character;

                float interval = CharacterInterval;
                if (_advanceAction.IsPressed()) interval = .01f;
                yield return new WaitForSeconds(interval);
            }
        }

        yield return new WaitUntil(() => _advanceAction.WasPressedThisFrame());

        _isPrintingDialog = false;
    }
}

