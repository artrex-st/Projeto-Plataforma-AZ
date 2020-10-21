using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestName : State
{
    public TestName(Character character) : base(character) { }
    public IEnumerator ChamaDebug()
    {
        Debug.Log($"antes de esperar");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("print depois de esperar");
        character.SetState(new IdleState(character));
    }
    public override void OnStateEnter()
    {
        character.StartCoroutine(ChamaDebug());
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStay()
    {
        Debug.Log($"está no Update do {GetType().Name} e executando no {character.name} com {2} ");
        base.OnStay();
    }
}
