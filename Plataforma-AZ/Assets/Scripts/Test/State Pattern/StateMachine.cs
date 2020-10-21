using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IStates activeState;
    public IStates preState;
    public void ChangeState(IStates newState)
    {
        if (activeState != null)
        {
            activeState.ExitState();
        }
        preState = activeState;
        activeState = newState;
        activeState.EnterState();
    }
    public void ExecuteActiveState()
    {
        if (activeState != null)
        {
            activeState.ExecuteState();
        }
    }

    public void ChangeToPreState()
    {
        activeState.ExitState();
        activeState = preState;
        activeState.EnterState();
    }
}
