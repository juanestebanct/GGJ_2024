using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput _input;

    [Header("Input Values")]
    public Vector2 MovementInput;
    public Vector2 LookInput;
    public Action OnJumpPressed;
    public Action OnFirePressed;
    public Action OnFireReleased;
    public Action<bool> OnSprintPressed;
    public Action<bool> OnSprintReleased;

    private void Awake()
    {
        _input = new PlayerInput();

        _input.Player.Jump.performed += context => OnJumpPressed?.Invoke();
        _input.Player.Fire.performed += context => OnFirePressed?.Invoke();
        _input.Player.Fire.canceled += context => OnFireReleased?.Invoke();
        _input.Player.Sprint.performed += context => OnSprintPressed?.Invoke(true);
        _input.Player.Sprint.canceled += context => OnSprintReleased?.Invoke(false);
    }

    private void Update()
    {
        MovementInput = _input.Player.Move.ReadValue<Vector2>();
        LookInput = _input.Player.Look.ReadValue<Vector2>();
    }

    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}
