using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyWanderState : PreyBaseState
{

    public override void EnterState(PreyStateManager prey)
    {
        // Set wander movement speed
        // Set a target
        prey.FishController.SetNewTarget(GenerateTarget());
        Debug.Log(prey.FishController.Target);
    }

    public override void ExitState(PreyStateManager prey)
    {
        // maybe increase speed for flee state
    }

    public override Vector3 GenerateTarget()
    {
        return new Vector3(447, 36, 245); 
    }

    public override void UpdateState(PreyStateManager prey)
    {
        if (prey.IsFleeing)
            prey.SwitchStates(prey.FleeState);
        // check if player is within range
        // switch to flee state if player is within range
        // otherwise move towards target
    }
}
