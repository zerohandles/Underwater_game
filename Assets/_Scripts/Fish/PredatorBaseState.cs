using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PredatorBaseState 
{
    public abstract void EnterState(PredatorStateMachine predator);
    public abstract void ExitState(PredatorStateMachine predator);
    public abstract void UpdateState(PredatorStateMachine predator);
    public abstract void GenerateTarget();
}
