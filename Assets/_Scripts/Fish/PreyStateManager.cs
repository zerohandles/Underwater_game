using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyStateManager : MonoBehaviour
{
    PreyBaseState _currentState;

    public bool IsWandering;
    public bool IsFleeing;

    public Fish FishController { get; private set; }

    public PreyWanderState WanderState = new PreyWanderState();
    public PreyFleeState FleeState = new PreyFleeState();


    private void OnEnable()
    {
        IsWandering = true;
        _currentState = WanderState;
        _currentState.EnterState(this);
    }
    void Awake()
    {
        FishController = GetComponent<Fish>();
    }

    void Update()
    {
        _currentState.UpdateState(this);    
    }

    public void SwitchStates(PreyBaseState state)
    {
        _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
        Debug.Log($"Switching to {_currentState} state");
    }
}
