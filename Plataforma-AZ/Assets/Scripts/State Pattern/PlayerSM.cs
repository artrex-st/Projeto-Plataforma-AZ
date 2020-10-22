using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    public StateMachine moveSM = new StateMachine();
    public StateMachine actionSM = new StateMachine();
    public Rigidbody2D playerBody;
    public float moveSpeed;
    public Vector2 moveAxis;
    public bool canMove;
    [Tooltip("Jump")]
    public float jumpForce;
    public Transform jumpFootPoint;
    public float jumpGroundRange;
    public LayerMask jumpGroundLayer;

    #region States triggrers
    private void TriggerMove()
    {
        moveSM.ChangeState(new MoveAxisState(playerBody, moveSpeed));
    }
    private void TriggerJump()
    {
        actionSM.ChangeState(new JumpState(playerBody, jumpForce, jumpFootPoint, jumpGroundRange, jumpGroundLayer));
    }
    #endregion
    void Start()
    {
        TriggerMove();
        TriggerJump();
    }
    void Update()
    {
        if (JumpGroundCheck() && Input.GetButtonDown("Jump"))
        {
            actionSM.ExecuteActiveState();
        }
        moveSM.ExecuteActiveState();
    }
    #region Variables
    private bool JumpGroundCheck()
    {
        return Physics2D.Raycast(jumpFootPoint.position, Vector2.down, jumpGroundRange, jumpGroundLayer);
    }
    #endregion
}
