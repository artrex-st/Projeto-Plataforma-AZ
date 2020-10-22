using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStates
{
    void EnterState();
    void ExitState();
    void ExecuteState();
}
