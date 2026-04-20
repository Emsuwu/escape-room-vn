using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeDetector : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputActionAsset;

    private InputAction _gamepadAction;
    private InputAction _gamepadAxisAction;
    private InputAction _keyboardAndMouseAction;

    private void Awake()
    {
        _gamepadAction = InputSystem.actions.FindAction("Gamepad");
        _gamepadAxisAction = InputSystem.actions.FindAction("GamepadAxis");
        _keyboardAndMouseAction = InputSystem.actions.FindAction("KeyboardAndMouse");
    }
    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("ControlScheme").Enable();
    }
    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("ControlScheme").Disable();
    }
    private void Update()
    {
        if (_gamepadAction.IsPressed() ) { Settings.IsUsingGamepad = true; }
        else if (_keyboardAndMouseAction.IsPressed() ) {  Settings.IsUsingGamepad = false; }
        else if ( _gamepadAxisAction.ReadValue<Vector2>().magnitude > 0.05f ) { Settings.IsUsingGamepad = true; }
    }
}
