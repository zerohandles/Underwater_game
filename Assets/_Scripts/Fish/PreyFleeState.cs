using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyFleeState : PreyBaseState
{
    public override void EnterState(PreyStateManager prey)
    {
        // Set flee speed
        // Set player as target
        prey.IsWandering = false;
        prey.FishController.SetNewTarget(GenerateTarget());
        Debug.Log(prey.FishController.Target);
    }

    public override void ExitState(PreyStateManager prey)
    {
        // Do exit stuff
    }

    public override Vector3 GenerateTarget()
    {
        return new Vector3(767, 85, 681);
    }

    public override void UpdateState(PreyStateManager prey)
    {
        if (prey.IsWandering)
            prey.SwitchStates(prey.WanderState);
        // check if player is within flee range
        // Move away from player if within flee range
        // else switch to wander state
    }
}
