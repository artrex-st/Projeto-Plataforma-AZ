using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IStates
{
    private Rigidbody2D jumpBody;
    private float jumpForce;
    private bool jumpIsGround;

    public JumpState(Rigidbody2D jumpBody, float jumpForce, bool jumpIsGround)
    {
        this.jumpBody = jumpBody;
        this.jumpForce = jumpForce;
        this.jumpIsGround = jumpIsGround;
    }

    public void EnterState()
    {
        Debug.Log($"Entrando no estado: {GetType().Name}");
    }

    public void ExecuteState()
    {
        if (jumpIsGround)
        {
            jumpBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void ExitState()
    {
        Debug.Log($"Saindo do estado: {GetType().Name}");
    }
}
