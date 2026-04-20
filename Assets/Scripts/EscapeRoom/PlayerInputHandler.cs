using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private InputActionAsset _inputActionAsset;
    private InputAction _interactAction;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Vector2 _lastMousePos;
    [SerializeField] private GameObject _movementUI;
    public static PlayerInputHandler Instance;
    private bool _isInputDisabled;
    public bool IsPaused;

    [Header("PuzzleInputs")]
    private InputAction _puzzleLeftAction;
    private InputAction _puzzleRightAction;
    private InputAction _puzzleDownAction;
    private InputAction _puzzleUpAction;
    private InputAction _puzzleBackAction;

    private InputAction _wAction;
    private InputAction _aAction;
    private InputAction _sAction;
    private InputAction _dAction;
    private InputAction _pauseAction;

    public bool IsInputDisabled { 
        get {return _isInputDisabled; } 
        set 
        { 
            _isInputDisabled = value;
            _movementUI.SetActive(!IsInputDisabled);
        } 
    }

    private void Awake()
    {
        Instance = this;
        _interactAction = InputSystem.actions.FindAction("Interact");
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");

        _wAction = InputSystem.actions.FindAction("W");
        _aAction = InputSystem.actions.FindAction("A");
        _sAction = InputSystem.actions.FindAction("S");
        _dAction = InputSystem.actions.FindAction("D");
        _pauseAction = InputSystem.actions.FindAction("Pause");

        _puzzleDownAction = InputSystem.actions.FindAction("PuzzleDown");
        _puzzleLeftAction = InputSystem.actions.FindAction("PuzzleLeft");
        _puzzleRightAction = InputSystem.actions.FindAction("PuzzleRight");
        _puzzleUpAction = InputSystem.actions.FindAction("PuzzleUp");
        _puzzleBackAction = InputSystem.actions.FindAction("PuzzleBack");
    }

    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("Player").Enable();
        Settings.UpdateSettings();
    }

    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("Player").Disable();
    }
    private void Update()
    {
        if (Puzzle.CurrentPuzzle != null) DoPuzzleInput();

        if (IsInputDisabled) return;
        if (IsPaused) return;

        SetControlInputType();
        CheckInteractAction();
        DoControllerCameraRotation();
        DoKeyboardCameraRotation();
        MoveReticle();
        CheckPause();
    }
    private void CheckPause()
    {
        if (Puzzle.CurrentPuzzle != null) return;
        if (_pauseAction.WasPressedThisFrame())
        {
            IsPaused = true;
            _menuRoot.SetActive(true);
            _settingsMenu.SetActive(true);
        }
    }
    private void DoPuzzleInput()
    {
        Puzzle.CurrentPuzzle.SendInput(_puzzleUpAction, _puzzleDownAction, _puzzleLeftAction, _puzzleRightAction, _puzzleBackAction);
    }
    private void SetControlInputType()
    {
        Vector2 mouseMovement = Mouse.current.position.ReadValue() - _lastMousePos;
        Vector2 moveAxis = _moveAction.ReadValue<Vector2>();

        bool isMouseMoving = Mathf.Abs(mouseMovement.magnitude) > 0.05f;
        bool isStickMoving = Mathf.Abs(moveAxis.magnitude) > 0.05;

        //if (isStickMoving) { Settings.IsUsingGamepad = true; }
        //if (isMouseMoving) { Settings.IsUsingGamepad = false; }
        if (Settings.IsUsingGamepad) { Reticle.Instance.gameObject.SetActive(true); }
        else { Reticle.Instance.gameObject.SetActive(false); }

        _lastMousePos = Mouse.current.position.ReadValue();
    }
    private void CheckInteractAction()
    {
        if (IsInputDisabled) return;
        if (Interactable.CurrentlyHovered == null) return;
        if (!_interactAction.WasPressedThisFrame()) return;
        Interactable.CurrentlyHovered.Interact();
        //Debug.Log("Interact!");
    }
    private void DoControllerCameraRotation()
    {
        Vector2 lookAxis = _lookAction.ReadValue<Vector2>();

        int lookYSign = Settings.InvertCamY ? -1 : 1;
        int lookXSign = Settings.InvertCamX ? -1 : 1;

        PlayerCamera.Instance.Rotate(
            new Vector3(
                lookYSign * -lookAxis.y * Settings.CameraSensitivity, 
                lookXSign * lookAxis.x * Settings.CameraSensitivity / 8, 
                0
            ));
    }
    private void DoKeyboardCameraRotation()
    {
        int lookYSign = Settings.InvertCamY ? -1 : 1;
        int lookXSign = Settings.InvertCamX ? -1 : 1;

        float w = _wAction.IsPressed() ? 1 : 0;
        float s = _sAction.IsPressed() ? 1 : 0;
        float a = _aAction.IsPressed() ? 1 : 0;
        float d = _dAction.IsPressed() ? 1 : 0;
        Vector2 lookAxis = new Vector2(lookXSign * (d-a), lookYSign * (w-s) * 4) / 8;

        PlayerCamera.Instance.Rotate(
            new Vector3(
                -lookAxis.y * Settings.CameraSensitivity,
                lookAxis.x * Settings.CameraSensitivity,
                0
            ));
    }
    private void MoveReticle()
    {
        int moveYSign = Settings.InvertReticleY ? -1 : 1;
        int moveXSign = Settings.InvertReticleX ? -1 : 1;

        Vector2 moveAxis = _moveAction.ReadValue<Vector2>();
        Reticle.Instance.Move(
            moveYSign * moveAxis.x * Settings.ReticleSensitivity, 
            moveXSign * moveAxis.y * Settings.ReticleSensitivity);
    }
}
