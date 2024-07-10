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
    [SerializeField] string _spint = "Sprint";

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

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
