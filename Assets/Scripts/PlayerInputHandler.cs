using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;
    private InputAction _interactAction;

    public bool IsInputDisabled;

    private void Awake()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
    }
    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        if (IsInputDisabled) return;

        if (_interactAction.WasPressedThisFrame() && Interactable.CurrentlyHovered != null)
        {
            Interactable.CurrentlyHovered.Interact();
        }
    }
}
