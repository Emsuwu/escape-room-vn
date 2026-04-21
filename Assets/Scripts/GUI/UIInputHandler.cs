using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    public static UIInputHandler Instance { get; private set; }

    [SerializeField] InputActionAsset _inputActionAsset;
   
    private InputAction _cancelAction;
    public delegate void OnCancelPressed();
    public event OnCancelPressed OnCancel;

    private InputAction _pauseAction;
    public delegate void OnPausePressed();
    public event OnPausePressed OnPause;
    private void Awake()
    {
        Instance = this;
        _cancelAction = _inputActionAsset.FindAction("Cancel");
        _pauseAction = _inputActionAsset.FindAction("Pause");
    }
    private void OnEnable()
    {
        _inputActionAsset.FindActionMap("UI").Enable();
    }
    private void OnDisable()
    {
        _inputActionAsset.FindActionMap("UI").Disable();
    }
    private void Update()
    {
        if (_cancelAction.WasPressedThisFrame()) Cancel();
        if (_pauseAction.WasPressedThisFrame()) Pause();
    }
    public void Cancel()
    {
        OnCancel?.Invoke();
    }
    public void Pause()
    {
        OnPause?.Invoke();
    }
}
