using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    [Header("PlayerController")] [SerializeField]
    public Transform Camera;

    [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
    [SerializeField, Range(2, 20)] float RunningSpeed = 4.0f;
    [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
    [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;

    [Space(10)] [Header("Advance")] [SerializeField]
    float RunningFOV = 65.0f;

    [SerializeField] float SpeedToFOV = 4.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float timeToRunning = 2.0f;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool CanRunning = true;

    [Space(10)]
    [Header("Input")] 
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;
    bool isCrough = false;
    float rotationX = 0;
    [HideInInspector] public bool isRunning = false;
    float InstallFOV;
    Camera cam;
    [HideInInspector] public bool Moving;
    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    [HideInInspector] public float Lookvertical;
    [HideInInspector] public float Lookhorizontal;
    private float RunningValue;
    private float installGravity;
    private bool WallDistance;
    [HideInInspector] public float WalkingValue;
    public float MouseSensitivity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InstallFOV = cam.fieldOfView;
        RunningValue = RunningSpeed;
        installGravity = gravity;
        WalkingValue = walkingSpeed;
    }

    void Update()
    {
        HandleCameraLook();
        HandleMovement();
    }


    private void HandleCameraLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked && canMove)
        {
            Lookvertical = -Input.GetAxis("Mouse Y");
            Lookhorizontal = Input.GetAxis("Mouse X");
            lookSpeed = MouseSensitivity;
            rotationX += Lookvertical * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            
            print(rotationX);
            Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

            if (isRunning && Moving)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
            else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        if (!characterController.isGrounded) moveDirection.y -= gravity * Time.deltaTime;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        isRunning = !isCrough ? CanRunning ? Input.GetKey(KeyCode.LeftShift) : false : false;
        vertical = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Vertical") : 0;
        horizontal = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Horizontal") : 0;
        if (isRunning) RunningValue = Mathf.Lerp(RunningValue, RunningSpeed, timeToRunning * Time.deltaTime);
        else RunningValue = WalkingValue;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * vertical) + (right * horizontal);

        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded) moveDirection.y = jumpSpeed;
        else moveDirection.y = movementDirectionY;

        characterController.Move(moveDirection * Time.deltaTime);
        Moving = horizontal < 0 || vertical < 0 || horizontal > 0 || vertical > 0 ? true : false;
    }
}