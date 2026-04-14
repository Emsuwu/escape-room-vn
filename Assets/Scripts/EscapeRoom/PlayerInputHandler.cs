using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;
    private InputAction _interactAction;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Vector2 _lastMousePos;
    [SerializeField] private GameObject _movementUI;
    public static PlayerInputHandler Instance;
    private bool _isInputDisabled;

    [Header("PuzzleInputs")]
    private InputAction _puzzleLeftAction;
    private InputAction _puzzleRightAction;
    private InputAction _puzzleDownAction;
    private InputAction _puzzleUpAction;
    private InputAction _puzzleBackAction;

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

        SetControlInputType();
        CheckInteractAction();
        DoControllerCameraRotation();
        MoveReticle();
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

        if (isStickMoving) { Settings.IsUsingGamepad = true; }
        if (isMouseMoving) { Settings.IsUsingGamepad = false; }
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

        PlayerCamera.Instance.Rotate(
            new Vector3(-lookAxis.y * Settings.CameraSensitivity * 2, 
            lookAxis.x * Settings.CameraSensitivity / 2, 
            0));
    }
    private void MoveReticle()
    {
        Vector2 moveAxis = _moveAction.ReadValue<Vector2>();
        Reticle.Instance.Move(
            moveAxis.x * Settings.ReticleSensitivity, 
            moveAxis.y * Settings.ReticleSensitivity);
    }
}
