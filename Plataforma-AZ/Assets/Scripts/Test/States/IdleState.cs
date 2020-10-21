using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : State
{
    [SerializeField]
    public IdleState(Character character) : base(character) { }

    public override void OnStateEnter()
    {
        Debug.Log("Entrou no Idle");
        character.StartCoroutine(MudarEstado());
    }
    private IEnumerator MudarEstado()
    {
        Debug.Log($"Vai mudar para o estado {GetType().Name} em 1 segundo;");
        yield return new WaitForSeconds(1);
        character.SetState(new TestName(character));

    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStay()
    {
        Debug.Log($"está no Update do {GetType().Name} e executando no {character.GetType().Name}");
        base.OnStay();
    }

}
