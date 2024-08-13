using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyFleeState : PreyBaseState
{
    Fish _fishController;

    public override void EnterState(PreyStateManager prey)
    {
        // Set flee speed
        // Set player as target
        _fishController = prey.FishController;
        prey.IsWandering = false;
        GenerateTarget();
        Debug.Log(prey.FishController.Target);
    }

    public override void ExitState(PreyStateManager prey)
    {
        // Do exit stuff
    }

    public override void GenerateTarget()
    {
        _fishController.SetRandomTarget();
    }

    public override void UpdateState(PreyStateManager prey)
    {
        if (prey.IsWandering)
            prey.SwitchStates(prey.WanderState);
        else if (_fishController.IsTargetReached)
            GenerateTarget();
    }
}
