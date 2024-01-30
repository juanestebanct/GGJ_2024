using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BabySway : MonoBehaviour
{
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;
    private InputManager _input;

    private void Start() => _input = InputManager.Instance;
    private void Update()
    {
        float mouseX = _input.LookInput.x * _swayMultiplier;
        float mouseY = _input.LookInput.y * _swayMultiplier;

        Quaternion xRotation = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(-mouseX, Vector3.up);

        Quaternion rotation = xRotation * yRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, _smooth * Time.deltaTime);
    }
}
