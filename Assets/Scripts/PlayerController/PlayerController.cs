using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController), typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [HideInInspector] public CharacterController Controller;
    private InputManager _inputManager;
    
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

    [Space(10)] 
    [Header("Input")] 
    [HideInInspector] public Vector3 MoveDirection = Vector3.zero;
    float rotationX = 0;
    [HideInInspector] public bool isRunning = false;
    float InstallFOV;
    Camera cam;
    [HideInInspector] public bool Moving;
    [HideInInspector] public float Vertical;
    [HideInInspector] public float Horizontal;
    [HideInInspector] public float LookVertical;
    [HideInInspector] public float LookHorizontal;
    private float _moveVertical;
    private float _moveHorizontal;
    private float RunningValue;
    private bool WallDistance;
    [HideInInspector] public float WalkingValue;
    public float MouseSensitivity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(transform.root);
        _inputManager = GetComponent<InputManager>();
    }

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InstallFOV = cam.fieldOfView;
        RunningValue = _runningSpeed;
        WalkingValue = _walkingSpeed;
    }

    void Update()
    {
        HandleCameraLook();
        HandleMovement();
        HandleInput();
    }

    private void HandleInput()
    {
        LookVertical = -_inputManager.LookInput.y;
        LookHorizontal = _inputManager.LookInput.x;
        _moveVertical = _inputManager.MovementInput.y;
        _moveHorizontal = _inputManager.MovementInput.x;

    }


    private void HandleCameraLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked && CanMove)
        {
            _lookSpeed = MouseSensitivity;
            rotationX += LookVertical * _lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -_lookXLimit, _lookXLimit);
            _camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, LookHorizontal * _lookSpeed, 0);

            if (isRunning && Moving)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _runningFOV, _speedToFOV * Time.deltaTime);
            else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, _speedToFOV * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        if (!Controller.isGrounded) MoveDirection.y -= _gravity * Time.deltaTime;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        isRunning = CanRun && Input.GetKey(KeyCode.LeftShift);
        Vertical = CanMove ? (isRunning ? RunningValue : WalkingValue) * _moveVertical : 0;
        Horizontal = CanMove ? (isRunning ? RunningValue : WalkingValue) * _moveHorizontal : 0;
        if (isRunning) RunningValue = Mathf.Lerp(RunningValue, _runningSpeed, _timeToRunning * Time.deltaTime);
        else RunningValue = WalkingValue;
        float movementDirectionY = MoveDirection.y;
        MoveDirection = (forward * Vertical) + (right * Horizontal);

        if (Input.GetKeyDown(KeyCode.Space) && CanMove && Controller.isGrounded) MoveDirection.y = _jumpSpeed;
        else MoveDirection.y = movementDirectionY;

        Controller.Move(MoveDirection * Time.deltaTime);
        Moving = Horizontal < 0 || Vertical < 0 || Horizontal > 0 || Vertical > 0 ? true : false;
    }
}