using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IStates
{
    private Rigidbody2D jumpBody;
    private float jumpForce;

    private bool jumpCheckIn;

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
    }

    public void ExecuteState()
    {
        if (JumpGroundCheck())
        {
            jumpBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
