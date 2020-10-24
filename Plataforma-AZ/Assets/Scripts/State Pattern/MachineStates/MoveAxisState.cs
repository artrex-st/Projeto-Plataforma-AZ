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
    }
    public void ExecuteState()
    {
        moveAxisBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveAxisSpeed, moveAxisBody.velocity.y);
    }
    public void ExitState()
    {
    }
}
