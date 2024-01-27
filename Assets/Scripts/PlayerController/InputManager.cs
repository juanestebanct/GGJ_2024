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
    public Action OnJumpReleased;
    public Action OnFirePressed;
    public Action OnFireReleased;

    private void Awake()
    {
        _input = new PlayerInput();

        _input.Player.Jump.performed += context => OnJumpPressed?.Invoke();
        _input.Player.Jump.canceled += context => OnJumpReleased?.Invoke();

        _input.Player.Fire.performed += context => OnFirePressed?.Invoke();
        _input.Player.Fire.canceled += context => OnFireReleased?.Invoke();
    }

    private void Update()
    {
        MovementInput = _input.Player.Move.ReadValue<Vector2>();
        LookInput = _input.Player.Look.ReadValue<Vector2>();
    }

    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}
