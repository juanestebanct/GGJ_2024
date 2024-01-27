using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [HideInInspector] public CharacterController Controller;
    private ReloadMiniGameController _reloadMiniGame;
    private InputManager _inputManager;
    private Camera _cam;
    
    [Header("PlayerController")] 
    [SerializeField] private Transform _camera;
    [SerializeField, Range(1, 10)] private float _walkingSpeed = 3.0f;
    [SerializeField, Range(2, 20)] private float _runningSpeed = 4.0f;
    [SerializeField, Range(0, 20)] private float _jumpSpeed = 6.0f;
    [SerializeField, Range(0.5f, 10)] private float _lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] private float _lookXLimit = 80.0f;

    [Space(10)] 
    [Header("Advance")] 
    [SerializeField] private float _runningFOV = 65.0f;
    [SerializeField] private float _speedToFOV = 4.0f;
    [SerializeField] private float _gravity = 20.0f;
    [SerializeField] private float _timeToRunning = 2.0f;
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public bool CanRun = true;
    private bool _jump;

    [Space(10)] 
    [Header("Input")] 
    [HideInInspector] public Vector3 MoveDirection = Vector3.zero;
    [HideInInspector] public bool IsRunning = false;
    [HideInInspector] public bool Moving;
    [HideInInspector] public float Vertical;
    [HideInInspector] public float Horizontal;
    [HideInInspector] public float LookVertical;
    [HideInInspector] public float LookHorizontal;
    [HideInInspector] public float WalkingValue;
    public float MouseSensitivity;
    private float _moveVertical;
    private float _moveHorizontal;
    private float _runningValue;
    private bool _wallDistance;
    private float _installFOV;
    private float _rotationX = 0;
    private bool _runInput;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(transform.root);
        _reloadMiniGame = GetComponent<ReloadMiniGameController>();
        _inputManager = GetComponent<InputManager>();
        _inputManager.OnJumpPressed += Jump;
        _inputManager.OnSprintPressed += Run;
        _inputManager.OnSprintReleased += Run;
    }

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _installFOV = _cam.fieldOfView;
        _runningValue = _runningSpeed;
        WalkingValue = _walkingSpeed;
        _jump = false;
    }

    void Update()
    {
        HandleCameraLook();
        HandleMovement();
        HandleInput();
    }

    private void HandleInput()
    {
        if (_reloadMiniGame.CurrentState == ReloadState.Reloading) LookHorizontal = 0;    
        else LookHorizontal = _inputManager.LookInput.x;
        LookVertical = -_inputManager.LookInput.y;
        _moveVertical = _inputManager.MovementInput.y;
        _moveHorizontal = _inputManager.MovementInput.x;
    }


    private void HandleCameraLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked && CanMove)
        {
            _lookSpeed = MouseSensitivity;
            _rotationX += LookVertical * _lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
            _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, LookHorizontal * _lookSpeed, 0);

            if (IsRunning && Moving)
                _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _runningFOV, _speedToFOV * Time.deltaTime);
            else _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _installFOV, _speedToFOV * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        if (!Controller.isGrounded) MoveDirection.y -= _gravity * Time.deltaTime;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        IsRunning = CanRun && _runInput;
        Vertical = CanMove ? (IsRunning ? _runningValue : WalkingValue) * _moveVertical : 0;
        Horizontal = CanMove ? (IsRunning ? _runningValue : WalkingValue) * _moveHorizontal : 0;
        if (IsRunning) _runningValue = Mathf.Lerp(_runningValue, _runningSpeed, _timeToRunning * Time.deltaTime);
        else _runningValue = WalkingValue;
        float movementDirectionY = MoveDirection.y;
        MoveDirection = (forward * Vertical) + (right * Horizontal);
        
        if (_jump && CanMove && Controller.isGrounded)
        {
            MoveDirection.y = _jumpSpeed;
            _jump = false;
        }
        else MoveDirection.y = movementDirectionY;

        Controller.Move(MoveDirection * Time.deltaTime);
        Moving = Horizontal < 0 || Vertical < 0 || Horizontal > 0 || Vertical > 0 ? true : false;
    }

    private void Jump() { _jump = true; }
    private void Run(bool value) { _runInput = value; } 
}