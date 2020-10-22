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

    // player
    public bool jumpRequest;

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
    }
    void Update()
    {
        moveSM.ExecuteActiveState();
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }
    }
    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            jumpRequest = false;
            TriggerJump();
        }
        actionSM.ExecuteActiveState();
    }
    #region Variables
    
    #endregion
}
