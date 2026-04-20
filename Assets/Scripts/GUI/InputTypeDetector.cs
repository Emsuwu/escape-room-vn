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
        _gamepadAction = _inputActionAsset.FindAction("Gamepad");
        _gamepadAxisAction = _inputActionAsset.FindAction("GamepadAxis");
        _keyboardAndMouseAction = _inputActionAsset.FindAction("KeyboardAndMouse");
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
