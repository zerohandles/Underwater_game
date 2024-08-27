using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

public class PredatorChaseState : PredatorBaseState
{
    Transform _player;
    Fish _fishController;
    float _targetTimer;

    public override void EnterState(PredatorStateMachine predator)
    {
        _player = predator.FishController.Player.transform;
        _fishController = predator.FishController;
        _fishController.Agent.speed = _fishController.ChaseSpeed;
        GenerateTarget();
        Debug.Log("Entering Chase State");
    }

    public override void ExitState(PredatorStateMachine predator)
    {
        // Exit code
        Debug.Log("Exiting Chase State");
    }

    public override void GenerateTarget()
    {
        Debug.Log("Setting new Target " + _player.position);
        _fishController.SetNewTarget(_player.position);
    }

    public override void UpdateState(PredatorStateMachine predator)
    {
        if (predator.IsAttacking)
            predator.SwitchStates(predator.AttackState);
        if (!predator.IsChasing)
            predator.SwitchStates(predator.WanderState);

        // Update nav agent destination at specific intervals
        _targetTimer += Time.deltaTime;
        if (_targetTimer >= 0.5f)
        {
            GenerateTarget();
            _targetTimer = 0;
        }
    }
}
