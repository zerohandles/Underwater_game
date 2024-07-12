using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("MovementSpeeds")]
    [SerializeField] float _swimSpeed = 3f;
    [SerializeField] float _sprintMultiplier = 2f;
    [SerializeField] float _ascendSpeed = 3f;
    [SerializeField] float _descendSpeed = 3f;

    [Header("Look Sensitivity")]
    [SerializeField] float _lookSensitivity = 100f;
    [SerializeField] float _upDownRange = 80f;

    [Header("Character Camera")]
    [SerializeField] Camera _followCamera;

    CharacterController _characterController;
    PlayerInputHandler _inputHandler;
    Vector3 _desiredMovement;
    float _verticalRotation;
    float _horizontalRotation;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputHandler = PlayerInputHandler.Instance;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        float mouseSensitivityMultiplier = _inputHandler.IsUsingMouse ? 0.3f : 1;

        _horizontalRotation += _inputHandler.LookInput.x * _lookSensitivity * mouseSensitivityMultiplier * Time.deltaTime;
        _verticalRotation += _inputHandler.LookInput.y * _lookSensitivity * mouseSensitivityMultiplier * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_upDownRange, _upDownRange);
        _horizontalRotation %= 360;
        transform.localEulerAngles = new Vector3(-_verticalRotation, _horizontalRotation, 0);
    }

    private void HandleMovement()
    {
        float speed = _swimSpeed * (_inputHandler.SprintValue > 0 ? _sprintMultiplier : 1);
        
        float depthChange = 0;
        if (_inputHandler.AscendValue > 0)
            depthChange += _ascendSpeed;
        else if (_inputHandler.DescendValue > 0)
            depthChange -= _descendSpeed;

        Vector3 inputDirection = new Vector3(_inputHandler.MoveInput.x, depthChange, _inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        _desiredMovement.x = worldDirection.x * speed;
        _desiredMovement.y = worldDirection.y * speed;
        _desiredMovement.z = worldDirection.z * speed;

        _characterController.Move(_desiredMovement * Time.deltaTime);
    }
}
