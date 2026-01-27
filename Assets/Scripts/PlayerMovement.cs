using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    [HideInInspector] public PlayerMovement Instance;
    private Rigidbody _rb;
    private PlayerInteract _playerInteractScript;

    public float WalkSpeed = 10f;
    public float CamSensitivity = 10f;
    public float CameraSensitivity;
    private InputAction _interactAction;
    private InputAction _moveAction;
    private Vector2 _moveAxis;
    private InputAction _lookAction;
    private Vector2 _lookAxis;

    void Awake()
    {
        if (Instance == null) Instance = this;
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _interactAction = InputSystem.actions.FindAction("Interact");
        _playerInteractScript = GetComponent<PlayerInteract>();
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }

    void FixedUpdate()
    {
        _moveAxis = _moveAction.ReadValue<Vector2>();
        _lookAxis = _lookAction.ReadValue<Vector2>();

        if (_interactAction.WasPressedThisFrame())
        {
            _playerInteractScript.Interact();
        }

        Movement();
    }

    private void Movement()
    {
        _rb.MovePosition(_rb.position + transform.forward * _moveAxis.y * WalkSpeed * Time.deltaTime);
        _rb.MovePosition(_rb.position + transform.forward * _moveAxis.x * WalkSpeed * Time.deltaTime);
    }
}
