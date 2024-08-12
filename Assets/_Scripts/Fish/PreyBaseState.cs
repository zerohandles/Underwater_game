using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreyBaseState
{
    public abstract void EnterState(PreyStateManager prey);
    public abstract void ExitState(PreyStateManager prey);
    public abstract void UpdateState(PreyStateManager prey);
    public abstract Vector3 GenerateTarget();
}
