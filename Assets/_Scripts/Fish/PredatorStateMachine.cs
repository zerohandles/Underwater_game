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
    public PredatorStats Stats { get; private set; }

    public PredatorAttackState AttackState = new PredatorAttackState();
    public PredatorChaseState ChaseState = new PredatorChaseState();
    public PredatorWanderState WanderState = new PredatorWanderState();

    void OnEnable()
    {
        _currentState = WanderState;
        _currentState.EnterState(this);
    }

    void Awake()
    {
        FishController = GetComponent<Fish>();
        Stats = GetComponent<PredatorStats>();
    }


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
