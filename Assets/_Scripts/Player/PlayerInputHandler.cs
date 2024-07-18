using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action")]
    [SerializeField] InputActionAsset _playerControls;

    [Header("Action Map Name References")]
    [SerializeField] string _actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] string _move = "Move";
    [SerializeField] string _look = "Look";
    [SerializeField] string _ascend = "Ascend";
    [SerializeField] string _descend = "Descend";
    [SerializeField] string _shoot = "Shoot";
    [SerializeField] string _sprint = "Sprint";

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _ascendAction;
    private InputAction _descendAction;
    private InputAction _shootAction;
    private InputAction _sprintAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float AscendValue { get; private set; }
    public float DescendValue { get; private set; }
    public float SprintValue { get; private set; }
    public bool ShootTriggered { get; private set; }
    public bool IsUsingMouse { get; private set; }


    public static PlayerInputHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _moveAction = _playerControls.FindActionMap(_actionMapName).FindAction(_move);
        _lookAction = _playerControls.FindActionMap(_actionMapName).FindAction(_look);
        _ascendAction = _playerControls.FindActionMap(_actionMapName).FindAction(_ascend);
        _descendAction = _playerControls.FindActionMap(_actionMapName).FindAction(_descend);
        _shootAction = _playerControls.FindActionMap(_actionMapName).FindAction(_shoot);
        _sprintAction = _playerControls.FindActionMap(_actionMapName).FindAction(_sprint);
        RegisterInputActions();
    }

    private void Update()
    {
        if (Gamepad.current != null)
            IsUsingMouse = false;
        else
            IsUsingMouse = true;
    }

    private void RegisterInputActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;

        _sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        _sprintAction.canceled += context => SprintValue = 0;

        _ascendAction.performed += context => AscendValue = context.ReadValue<float>();
        _ascendAction.canceled += context => AscendValue = 0;

        _descendAction.performed += context => DescendValue = context.ReadValue<float>();
        _descendAction.canceled += context => DescendValue = 0;

        _shootAction.performed += context => ShootTriggered = true;
        _shootAction.performed += context => ShootTriggered = false;
    }

    void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _ascendAction.Enable();
        _descendAction.Enable();
        _shootAction.Enable();
        _sprintAction.Enable();
    }

    void OnDisable()
    {
        _moveAction?.Disable();
        _lookAction?.Disable();
        _ascendAction?.Disable();
        _descendAction?.Disable();
        _shootAction?.Disable();
        _sprintAction?.Disable();
    }
}
