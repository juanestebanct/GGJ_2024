using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    private PlayerInput _input;

    [Header("Input Values")]
    public Vector2 MovementInput;
    public Vector2 LookInput;
    public Vector2 ReloadMovementInput;
    public Action OnJumpPressed;
    public Action OnPausePressed;
    public Action<bool> OnFirePressed;
    public Action<bool> OnFireReleased;
    public Action<bool> OnSprintPressed;
    public Action<bool> OnSprintReleased;
    public Action<bool> OnReloadPressed;
    public Action<bool> OnReloadReleased;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        _input = new PlayerInput();

        _input.Player.Jump.performed += context => OnJumpPressed?.Invoke();
        _input.Player.Pause.performed += context => OnPausePressed?.Invoke();
        _input.Player.Fire.performed += context => OnFirePressed?.Invoke(true);
        _input.Player.Fire.canceled += context => OnFireReleased?.Invoke(false);
        _input.Player.Sprint.performed += context => OnSprintPressed?.Invoke(true);
        _input.Player.Sprint.canceled += context => OnSprintReleased?.Invoke(false);
        _input.Player.Reload.performed += context => OnReloadPressed?.Invoke(true);
        _input.Player.Reload.canceled += context => OnReloadReleased?.Invoke(false);
    }

    private void Update()
    {
        MovementInput = _input.Player.Move.ReadValue<Vector2>();
        LookInput = _input.Player.Look.ReadValue<Vector2>();
        ReloadMovementInput = _input.Player.ReloadMovement.ReadValue<Vector2>();
    }

    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}
