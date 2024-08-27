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
        _fishController.Agent.speed = _fishController.FleeSpeed;
        prey.IsWandering = false;
        GenerateTarget();
        Debug.Log("Entering Flee State");
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
