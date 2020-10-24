using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : IStates
{
    private bool jumpCheckIn;

    private Rigidbody2D jumpBody;
    private float inputX;
    private float jumpForceX;
    private float jumpForceY;

    private Transform jumpWallPoint;
    private float jumpGroundCheckRange;
    private LayerMask jumpGroundLayer;

    public WallJumpState(Rigidbody2D jumpBody,float inputX, float jumpForceX,float jumpForceY, Transform jumpFootPoint, float jumpGroundCheckRange, LayerMask jumpGroundLayer)
    {
        this.jumpBody = jumpBody;
        this.inputX = inputX;
        this.jumpForceX = jumpForceX;
        this.jumpForceY = jumpForceY;
        this.jumpWallPoint = jumpFootPoint;
        this.jumpGroundCheckRange = jumpGroundCheckRange;
        this.jumpGroundLayer = jumpGroundLayer;
    }
    public void EnterState()
    {
        jumpCheckIn = true;
    }
    public void ExecuteState()
    {
        if (jumpCheckIn)
        {
            jumpBody.AddForce(new Vector2(jumpForceX * inputX, jumpForceY), ForceMode2D.Impulse);
            jumpCheckIn = false;
        }
    }
    public void ExitState()
    {

    }
    #region Variables
    private bool JumpWallCheck()
    {
        return Physics2D.Raycast(jumpWallPoint.position, Vector2.right * Input.GetAxisRaw("Horizontal"), jumpGroundCheckRange, jumpGroundLayer);
    }
    #endregion
}
