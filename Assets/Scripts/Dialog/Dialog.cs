using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TextAsset _file;
    private Coroutine _currentSequenceCoroutine;
    public float CharacterInterval = 0.025f;
    [HideInInspector] public float DefaultCharacterInterval;
    private bool _executionPaused;
    public static Dialog Instance;

    [SerializeField] private TMPro.TMP_Text _dialogText;
    [SerializeField] private TMPro.TMP_Text _characterNameText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _choiceBox;

    private List<GameObject> _choiceList = new List<GameObject>();
    
    //[SerializeField] private RectTransform _characterNameBoxRectTransform;

    [SerializeField] InputActionAsset _inputActionAsset;
    InputAction _advanceAction;

    private void Awake()
    {
        Instance = this;
        _advanceAction = InputSystem.actions.FindAction("Advance");
    }
    private void Start()
    {
        DefaultCharacterInterval = CharacterInterval;
        if (_file == null) return;
        DialogParser.LoadDialog(_file.text);
        Branch("unicorn_1");
        StartSequence();

        //SetBackground()
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

    public void ClearChoicesAndContinue()
    {
        foreach (GameObject choice in _choiceList) 
        {
            Destroy(choice);
        }
        _choiceList.Clear();
        _executionPaused = false;
    }
    /// <summary>
    /// Adds a choice to the choice box
    /// </summary>
    /// <param name="choiceText"></param>
    /// <param name="command"></param>
    public void AddChoice(string choiceText, DialogCommand command)
    {
        GameObject choiceButton = Instantiate(_choiceButtonPrefab);
        choiceButton.transform.SetParent(_choiceBox.transform, false);
        DialogChoiceButton choiceButtonScript = choiceButton.GetComponent<DialogChoiceButton>();
        choiceButton.GetComponent<DialogChoiceButton>().Command = command;
        choiceButtonScript.Text = choiceText;
        _choiceList.Add(choiceButton);
    }
    public void PauseExecution(bool pause = true)
    {
        _executionPaused = pause;
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

            current_command.Execute();
            yield return new WaitUntil(() => !_executionPaused);

        }
    }
    public void Wait(float seconds)
    {
        _executionPaused = true;
        StartCoroutine(CoWaitSeconds(seconds));
    }
    private IEnumerator CoWaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _executionPaused = false;
    }

    public void SetBackground(string path, bool absolute = false)
    {
        Debug.Log("Assets/Sprites/Backgrounds/" + path);
        Sprite texture;
        if (!absolute) texture = Resources.Load<Sprite>("Backgrounds/"+path);
        else texture = Resources.Load<Sprite>(path);
        _backgroundImage.sprite = texture;
    }

    public void ClearDialog()
    {
        _dialogText.text = string.Empty;
    }

    public void SetCharacterName(string characterName)
    {
        _characterNameText.text = characterName;
        //LayoutRebuilder.ForceRebuildLayoutImmediate(_characterNameBoxRectTransform);
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
    public void PrintDialog(string[] dialogArray, bool clearLastDialog = true)
    {
        _executionPaused = true;
        StartCoroutine(CoPrintDialog(dialogArray));
    }
    private IEnumerator CoPrintDialog(string[] dialogArray, bool clearLastDialog = true)
    {
        if (clearLastDialog) _dialogText.text = string.Empty;
        for (int i = 0; i < dialogArray.Length; i++)
        {
            string dialog = dialogArray[i];
            
            // If oddly numbered argument, interpret as a wait time
            if (i % 2 != 0)
            {
                if (float.TryParse(dialog, out float waitTime))
                {
                    yield return new WaitForSeconds(waitTime);
                    continue;
                }
                // If not a number
                Debug.LogError("Oddly numbered arguments in a dialog command should be wait times");
                Debug.Break();
            }
            // If evenly numbered argument, interpret as dialog to add
            foreach (char character in dialog)
            {
                _dialogText.text += character;

                float interval = CharacterInterval;
                if (_advanceAction.IsPressed()) interval = .01f;
                yield return new WaitForSeconds(interval);
            }
        }

        yield return new WaitUntil(() => _advanceAction.WasPressedThisFrame());

        _executionPaused = false;
    }
}

