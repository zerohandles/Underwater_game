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
    [SerializeField] float _mouseSensitivity = 2f;
    [SerializeField] float _upDownRange = 80f;

    [Header("Character Camera")]
    [SerializeField] Camera _followCamera;

    [Header("Underwater Movement Limits")]
    [SerializeField] float _waterSurfaceY = 103.5f;

    CharacterController _characterController;
    PlayerInputHandler _inputHandler;
    Vector3 _desiredMovement;
    float _verticalRotation;

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
        float mouseXRotation = _inputHandler.LookInput.x * _mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        
        _verticalRotation -= _inputHandler.LookInput.y * _mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_upDownRange, _upDownRange);

        _followCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
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

        // Ensure player stays below the surface of the water.
        if (transform.position.y > _waterSurfaceY)
            transform.position = new Vector3(transform.position.x, _waterSurfaceY, transform.position.z);
    }
}
