using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorWanderState : PredatorBaseState
{
    Fish _fishController;
    public override void EnterState(PredatorStateMachine predator)
    {
        _fishController = predator.FishController;
        GenerateTarget();
        Debug.Log("Entering Wander State");
    }

    public override void ExitState(PredatorStateMachine predator)
    {
        Debug.Log("Exiting Wander State");
    }

    public override void GenerateTarget()
    {
        _fishController.SetRandomTarget();
    }

    public override void UpdateState(PredatorStateMachine predator)
    {
        if (predator.IsChasing)
            predator.SwitchStates(predator.ChaseState);
        else if (_fishController.IsTargetReached)
            GenerateTarget();
    }
}
