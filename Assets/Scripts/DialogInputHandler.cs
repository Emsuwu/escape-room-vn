using UnityEngine;
using UnityEngine.InputSystem;

public class DialogInputHandler : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputActionAsset;
    InputAction _advanceAction;

    private void Awake()
    {
        _advanceAction = InputSystem.actions.FindAction("Advance");
    }
    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("Dialog").Enable();
    }
    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("Dialog").Disable();
    }
}
