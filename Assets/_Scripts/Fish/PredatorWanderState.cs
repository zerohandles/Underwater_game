using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorWanderState : PredatorBaseState
{
    public override void EnterState(PredatorStateMachine predator)
    {
        predator.FishController.SetNewTarget(GenerateTarget());
        Debug.Log("Entering Wander State");
    }

    public override void ExitState(PredatorStateMachine predator)
    {
        Debug.Log("Exiting Wander State");
    }

    public override Vector3 GenerateTarget()
    {
        // TODO: Add code the generate a random location
        return new Vector3(700, 75, 450);
    }

    public override void UpdateState(PredatorStateMachine predator)
    {
        if (predator.IsChasing)
            predator.SwitchStates(predator.ChaseState);
    }
}
