using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAttackState : PredatorBaseState
{
    [SerializeField] float _attackDelay = 3;
    float _attackTimer;
    float _targetTimer;
    Transform _player;
    Fish _fishController;

    public override void EnterState(PredatorStateMachine predator)
    {
        _fishController = predator.FishController;
        _attackTimer = 0;
        _player = predator.FishController.Player.transform;
        Debug.Log("Entering Attack State");
    }

    public override void ExitState(PredatorStateMachine predator)
    {
        // Exit code
        Debug.Log("Exiting Attack State");
    }

    public override void GenerateTarget()
    {
        Debug.Log("Setting new Attack target " + _player.position);
        _fishController.SetNewTarget(_player.position);
    }

    public override void UpdateState(PredatorStateMachine predator)
    {
        if (predator.IsChasing && !predator.IsAttacking)
        {
            predator.SwitchStates(predator.ChaseState);
        }
        if (!predator.IsChasing && !predator.IsAttacking)
        {
            predator.SwitchStates(predator.WanderState);
        }

        // Update nav mesh target at set intervals to prevent performance loss
        _targetTimer += Time.deltaTime;
        if (_targetTimer >= 0.5f)
        {
            GenerateTarget();
            _targetTimer = 0;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackDelay)
        {
            // Attack Code
            Debug.Log(predator.name + "Attacks");
        }
    }
}
