using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorStateMachine : MonoBehaviour
{
    PredatorBaseState _currentState;
    public bool IsAttacking;
    public bool IsChasing;

    public Fish FishController { get; private set; }

    public PredatorAttackState AttackState = new PredatorAttackState();
    public PredatorChaseState ChaseState = new PredatorChaseState();
    public PredatorWanderState WanderState = new PredatorWanderState();

    // Start is called before the first frame update
    void Start()
    {
        FishController = GetComponent<Fish>();
        _currentState = WanderState;
        _currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchStates(PredatorBaseState state)
    {
        _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }
}
