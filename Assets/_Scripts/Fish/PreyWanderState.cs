using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyWanderState : PreyBaseState
{
    Fish _fishController;

    public override void EnterState(PreyStateManager prey)
    {
        // Set wander movement speed?
        _fishController = prey.FishController;
        GenerateTarget();
    }

    public override void ExitState(PreyStateManager prey)
    {
        // increase speed for flee state?
    }

    public override void GenerateTarget() => _fishController.SetRandomTarget();

    public override void UpdateState(PreyStateManager prey)
    {
        if (prey.IsFleeing)
            prey.SwitchStates(prey.FleeState);
        else if (_fishController.IsTargetReached)
            GenerateTarget();
    }
}
