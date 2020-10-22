using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IStates
{
    private bool jumpCheckIn;

    private Rigidbody2D jumpBody;
    private float jumpForce;

    private Transform jumpFootPoint;
    private float jumpGroundCheckRange;
    private LayerMask jumpGroundLayer;

    public JumpState(Rigidbody2D jumpBody, float jumpForce, Transform jumpFootPoint, float jumpGroundCheckRange, LayerMask jumpGroundLayer)
    {
        this.jumpBody = jumpBody;
        this.jumpForce = jumpForce;
        this.jumpFootPoint = jumpFootPoint;
        this.jumpGroundCheckRange = jumpGroundCheckRange;
        this.jumpGroundLayer = jumpGroundLayer;
    }

    public void EnterState()
    {
        Debug.Log($"Entrando no estado: {GetType().Name}");
        jumpCheckIn = true;
    }

    public void ExecuteState()
    {
        if (jumpCheckIn && JumpGroundCheck())
        {
            jumpBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCheckIn = false;
        }
    }
    public void ExitState()
    {
        Debug.Log($"Saindo do estado: {GetType().Name}");
    }

    #region Variables
    private bool JumpGroundCheck()
    {
        return Physics2D.Raycast(jumpFootPoint.position, Vector2.down, jumpGroundCheckRange, jumpGroundLayer);
    }
    #endregion
}
