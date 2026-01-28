using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset _inputActions;
    [HideInInspector] public PlayerMovement Instance;
    [SerializeField] private Camera _camera;
    private Rigidbody _rb;
    private PlayerInteract _playerInteractScript;

    [Header("Settings")]
    public float WalkSpeed = 10f;
    [Range(0.1f,100f)] public float CameraSensitivity = 10f;
    [Header("Options for perverts")]
    public bool InvertX;
    public bool InvertY;
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
        Cursor.lockState = CursorLockMode.Locked;
        _rb.MovePosition(_rb.position + transform.forward * _moveAxis.y * WalkSpeed * Time.deltaTime);
        _rb.MovePosition(_rb.position + transform.right * _moveAxis.x * WalkSpeed * Time.deltaTime);

        float playerYRot = _lookAxis.x * CameraSensitivity * Time.deltaTime;
        float camXRot = -_lookAxis.y * CameraSensitivity * Time.deltaTime;
        if (InvertX) playerYRot *= -1;
        if (InvertY) camXRot *= -1;
        transform.Rotate(0, playerYRot, 0);
        _camera.transform.Rotate(camXRot, 0, 0);

        //Debug.Log($"_lookAxis: {_lookAxis} | xRot: {camXRot} | transform.rotation: {transform.rotation}");
    }
}
