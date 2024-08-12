using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

public class PredatorChaseState : PredatorBaseState
{
    Transform _player;
    float _targetTimer;

    public override void EnterState(PredatorStateMachine predator)
    {
        _player = predator.FishController.Player.transform;
        predator.FishController.SetNewTarget(GenerateTarget());
        Debug.Log("Entering Chase State");
    }

    public override void ExitState(PredatorStateMachine predator)
    {
        // Exit code
        Debug.Log("Exiting Chase State");
    }

    public override Vector3 GenerateTarget()
    {
        Debug.Log("Setting new Target " + _player.position);
        return _player.position;
    }

    public override void UpdateState(PredatorStateMachine predator)
    {
        if (predator.IsAttacking)
            predator.SwitchStates(predator.AttackState);
        if (!predator.IsChasing)
            predator.SwitchStates(predator.WanderState);

        _targetTimer += Time.deltaTime;
        if (_targetTimer >= 1)
        {
            predator.FishController.SetNewTarget(GenerateTarget());
            _targetTimer = 0;
        }
    }
}
