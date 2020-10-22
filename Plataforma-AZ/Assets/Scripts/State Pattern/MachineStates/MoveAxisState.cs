using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAxisState : IStates
{
    private Rigidbody2D moveAxisBody;
    private float moveAxisSpeed;
    private float moveAxisDirection;
    public MoveAxisState(Rigidbody2D moveAxisBody, float moveAxisSpeed)
    {
        this.moveAxisBody = moveAxisBody;
        this.moveAxisSpeed = moveAxisSpeed;
    }
    public void EnterState()
    {
        Debug.Log($"Entrando no estado: {GetType().Name}");
    }
    public void ExecuteState()
    {
        moveAxisBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveAxisSpeed, moveAxisBody.velocity.y);
    }
    public void ExitState()
    {
        Debug.Log($"Saindo do estado: {GetType().Name}");
    }
}
