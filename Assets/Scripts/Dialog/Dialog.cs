using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    private Coroutine _currentSequenceCoroutine;
    [HideInInspector] public float DefaultCharacterInterval;
    private bool _executionPaused;
    public static Dialog Instance;

    [Header("Settings")]
    [SerializeField] public float CharacterInterval = 0.025f;
    [Header("Initial dialog file")]
    [SerializeField] private TextAsset _file;

    [Header("Dialog UI Elements")]
    // Dialog elements
    [SerializeField] private Image _dialogBoxImage;
    [SerializeField] private Image _dialogBoxLineImage;
    [SerializeField] private GameObject _characterNameBox;
    [SerializeField] private Image _characterNameBoxImage;
    // Dialog text elements
    [SerializeField] private TMPro.TMP_Text _dialogText;
    [SerializeField] private TMPro.TMP_Text _characterNameText;

    [Header("Other UI Elements")]
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _choiceBox;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _transitionBackgroundImage;

    [Header("...")]
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

        _transitionBackgroundImage.color = new Color32(255, 255, 255, 255);
        _transitionBackgroundImage.CrossFadeAlpha(0, 0, false);
    }

    /// <summary>
    /// Shows the dialog UI
    /// </summary>
    /// <param name="time">Optional fade in time</param>
    /// <param name="wait">Whether to pause execution until crossfade ends</param>
    public void ShowDialog(float time = 0f, bool wait = true)
    {
        FadeDialog(1f, time, wait);
    }
    /// <summary>
    /// Hides the dialog UI
    /// </summary>
    /// <param name="time">Optional fade out time</param>
    /// <param name="wait">Whether to pause execution until crossfade ends</param>
    public void HideDialog(float time = 0f, bool wait = true)
    {
        FadeDialog(0f, time, wait);
    }
    public void FadeDialog(float alpha, float time, bool wait = true)
    {
        _dialogBoxImage.CrossFadeAlpha(alpha, time, false);
        _dialogBoxLineImage.CrossFadeAlpha(alpha, time, false);
        _characterNameBoxImage.CrossFadeAlpha(alpha, time, false);
        _dialogText.CrossFadeAlpha(alpha, time, false);
        _characterNameText.CrossFadeAlpha(alpha, time, false);

        if (wait) PauseExecutionForSeconds(time);
    }

    /// <summary>
    /// Pauses execution of commands in command queue for specified duration
    /// </summary>
    /// <param name="seconds">Duration to pause execution in seconds</param>
    public void PauseExecutionForSeconds(float seconds)
    {
        _executionPaused = true;
        StartCoroutine(CoPauseExecutionForSeconds(seconds));
    }
    private IEnumerator CoPauseExecutionForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _executionPaused = false;
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
    /// Removes all dialog choice buttons and unpauses execution
    /// </summary>
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
        StopAllCoroutines();
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

    /// <summary>
    /// Pauses the execution of commands for a duration
    /// </summary>
    /// <param name="seconds">The duration to pause execution</param>
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

    /// <summary>
    /// Sets the background color with r g b values
    /// </summary>
    /// <param name="r">Red value</param>
    /// <param name="g">Green value</param>
    /// <param name="b">Blue value</param>
    public void SetBackgroundColor(int r, int g, int b)
    {
        SetBackgroundColor(new Color32((byte)r, (byte)g, (byte)b, 255));
    }
    /// <summary>
    /// Sets the background color with a Color32 object
    /// </summary>
    /// <param name="color">Color32 object. E.g., new Color32(255, 255, 255, 255)</param>
    public void SetBackgroundColor(Color32 color)
    {
        _backgroundImage.sprite = null;
        _backgroundImage.color = color;
    }
    /// <summary>
    /// Sets the background with a path )
    /// </summary>
    /// <param name="path">Path to image (Resources/Backgrounds/{path}</param>
    /// <param name="absolute">If true, path starts at Resources rather than Backgrounds</param>
    public void SetBackground(string path, bool absolute = false)
    {
        _backgroundImage.color = Color.white;
        Sprite texture;
        if (!absolute) texture = Resources.Load<Sprite>("Backgrounds/"+path);
        else texture = Resources.Load<Sprite>(path);
        _backgroundImage.sprite = texture;
    }
    /// <summary>
    /// Sets the image of the transitional background
    /// </summary>
    /// <param name="path">Path to the image (Resources/Backgrounds/{path}</param>
    public void SetTransitionBackgroundImage(string path)
    {
        _transitionBackgroundImage.color = new Color32(255, 255, 255, 255);
        _transitionBackgroundImage.CrossFadeAlpha(0, 0, false);
        Sprite texture;
        texture = Resources.Load<Sprite>("Backgrounds/" + path);
        _transitionBackgroundImage.sprite = texture;
    }
    /// <summary>
    /// Sets the color of the transitional background
    /// </summary>
    /// <param name="r">Red value</param>
    /// <param name="g">Green value</param>
    /// <param name="b">Blue value</param>
    public void SetTransitionBackgroundColor(int r, int g, int b)
    {
        SetTransitionBackgroundColor(new Color32((byte)r, (byte)g, (byte)b, 255));
        _transitionBackgroundImage.CrossFadeAlpha(0, 0, false);
    }
    /// <summary>
    /// Sets the color of the transitional background using a Color32 object
    /// </summary>
    /// <param name="color">Color32 object e.g., new Color32(255, 255, 255, 255)</param>
    public void SetTransitionBackgroundColor(Color32 color)
    {
        _transitionBackgroundImage.sprite = null;
        _transitionBackgroundImage.color = color;
    }
    /// <summary>
    /// Crossfades the background to the image at the specified path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="duration"></param>
    /// <param name="no_pause">If true other commands can be executed without waiting for this to finish</param>
    public void CrossfadeBackgroundImage(string path, float duration, bool no_pause = false)
    {
        _executionPaused = !no_pause;
        SetTransitionBackgroundImage(path);
        _transitionBackgroundImage.CrossFadeAlpha(1, duration, false);
        StartCoroutine(CoCrossfadeBackroundImage(path, duration));
    }
    private IEnumerator CoCrossfadeBackroundImage(string path, float duration)
    {
        yield return new WaitForSeconds(duration);
        _executionPaused = false;
        SetBackground(path);
        _transitionBackgroundImage.CrossFadeAlpha(0, 0, false);
    }
    /// <summary>
    /// Crossfades the background's color
    /// </summary>
    /// <param name="r">Red value</param>
    /// <param name="g">Green value</param>
    /// <param name="b">Blue value</param>
    /// <param name="duration">Duration of crossfade</param>
    public void CrossfadeBackgroundColor(int r, int g, int b, float duration)
    {
        _executionPaused = true;
        SetTransitionBackgroundColor(r, g, b);
        _transitionBackgroundImage.CrossFadeAlpha(1, duration, false);
        StartCoroutine(CoCrossfadeBackroundColor(r, g, b, duration));
    }
    private IEnumerator CoCrossfadeBackroundColor(int r, int g, int b, float duration)
    {
        yield return new WaitForSeconds(duration);
        _executionPaused = false;
        SetBackgroundColor(r, g, b);
        _transitionBackgroundImage.CrossFadeAlpha(0, 0, false);
    }

    /// <summary>
    /// Clears the dialog box text
    /// </summary>
    public void ClearDialog()
    {
        _dialogText.text = string.Empty;
    }
    /// <summary>
    /// Sets the character name box text
    /// </summary>
    /// <param name="characterName">Character name to be displayed</param>
    public void SetCharacterName(string characterName)
    {
        _characterNameBox.SetActive(true);
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
    public void HideName()
    {
        _characterNameBox.SetActive(false);
    }
    public void CleanUp()
    {
        DialogParser.CurrentDialogSequence.DialogCommands.Clear();
        StopAllCoroutines();
    }
}

