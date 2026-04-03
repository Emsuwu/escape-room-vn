using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;
    private InputAction _interactAction;
    private InputAction _moveAction;
    private InputAction _lookAction;

    public bool IsInputDisabled;

    private void Awake()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
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
        if (IsInputDisabled) return;

        CheckInteractAction();
        DoControllerCameraRotation();
        MoveReticle();
    }

    private void CheckInteractAction()
    {
        if (Interactable.CurrentlyHovered == null) return;
        if (!_interactAction.WasPressedThisFrame()) return;
        Interactable.CurrentlyHovered.Interact();
        Debug.Log("Interact!");
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
