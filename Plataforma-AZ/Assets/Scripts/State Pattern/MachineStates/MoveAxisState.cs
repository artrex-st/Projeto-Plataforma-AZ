using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAxisState : IStates
{
    private Rigidbody2D moveAxisBody;
    private float moveAxisSpeed;
    private bool moveAxisCanMove;
    public MoveAxisState(Rigidbody2D moveAxisBody, float moveAxisSpeed, bool moveAxisCanMove)
    {
        this.moveAxisBody = moveAxisBody;
        this.moveAxisSpeed = moveAxisSpeed;
        this.moveAxisCanMove = moveAxisCanMove;
    }
    public void EnterState()
    {
        Debug.Log($"Entrando no estado: {GetType().Name}");
    }
    public void ExecuteState()
    {
        if (moveAxisCanMove)
        {
            moveAxisBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveAxisSpeed, moveAxisBody.velocity.y);
        }
    }
    public void ExitState()
    {
        Debug.Log($"Saindo do estado: {GetType().Name}");
    }
}
