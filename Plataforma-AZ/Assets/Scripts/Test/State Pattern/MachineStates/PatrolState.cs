using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IStates
{
    public void EnterState()
    {
        Debug.Log("entrando Patroling");
    }

    public void ExecuteState()
    {
        Debug.Log("Patroling");
    }

    public void ExitState()
    {
        Debug.Log("Saindo Patroling");
    }
}
