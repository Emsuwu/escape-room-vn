using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset _inputActions;
    [HideInInspector] public static PlayerMovement Instance;
    [SerializeField] private Camera _camera;
    private Rigidbody _rb;
    private PlayerInteract _playerInteractScript;
    [Header("Migrate to Settings later")]
    [Range(.5f, 10f)] public float WalkSpeed = 10f;
    [Range(0.01f, .5f)] public float CameraSensitivity = 10f;
    public float RefreshRate = 60f;
    public bool InvertX;
    public bool InvertY;
    private InputAction _interactAction;
    private InputAction _moveAction;
    private Vector2 _moveAxis;
    private InputAction _lookAction;
    private Vector2 _lookAxis;

    private float _lookPlayerY;
    private float _lookCamX;

    void Awake()
    {
        _lookCamX = _camera.transform.rotation.x;
        _lookPlayerY = transform.rotation.y;
        if (Instance == null) Instance = this;
        else { Debug.LogWarning("Hey! There should only be one Player at one time!"); }
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _interactAction = InputSystem.actions.FindAction("Interact");
        _playerInteractScript = GetComponent<PlayerInteract>();
        _rb = GetComponent<Rigidbody>();

        // Get refresh rate
        RefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        if (RefreshRate > 0)
        {
            Time.fixedDeltaTime = 1f / RefreshRate;
        }
    }
    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }
    void Update()
    {
        if (_interactAction.WasPressedThisFrame()) { _playerInteractScript.Interact(); }
    }
    private void LateUpdate()
    {
        Rotation();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        _moveAxis = _moveAction.ReadValue<Vector2>();
        Vector3 movement = (transform.forward * _moveAxis.y + transform.right * _moveAxis.x) * WalkSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movement);
    }
    private void Rotation()
    {
        _lookAxis = _lookAction.ReadValue<Vector2>();
        Cursor.lockState = CursorLockMode.Locked;

        float playerYRot = _lookAxis.x * CameraSensitivity;
        float camXRot = -_lookAxis.y * CameraSensitivity;
        if (InvertX) playerYRot *= -1;
        if (InvertY) camXRot *= -1;

        _lookPlayerY += playerYRot;
        _lookCamX += camXRot;
        _lookCamX = Mathf.Clamp(_lookCamX, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, _lookPlayerY, 0);
        _camera.transform.localRotation = Quaternion.Euler(_lookCamX, 0, 0);
    }
}
