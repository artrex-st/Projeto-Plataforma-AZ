using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    public StateMachine moveSM = new StateMachine();
    public StateMachine actionSM = new StateMachine();
    public Rigidbody2D playerBody;
    public float moveSpeed;
    //public Vector2 moveAxis;
    [Header("Player")]
    // player
    public float inputX;
    public bool jumpRequest;
    public bool canMove;

    [Header("Boolean's")]
    public bool isFliped;
    public bool isGround;
    public bool isWall;
    public bool canWallJump;
    public bool onStun;
    
    [Header("Jump")]
    public float jumpForce;
    public Transform jumpFootPoint;
    public float jumpGroundRange;
    public LayerMask jumpGroundLayer;
    
    [Header("Wall Jump")]
    public float wJForceX;
    public float wJForceY;
    public Transform wJPoint;
    public float wJRange;
    public LayerMask wJLayer;
    public float wJSlideDownSpeed;

    [Header("Testes")]
    public float CD;
    public bool test;

    #region States triggrers
    private void TriggerMove()
    {
        moveSM.ChangeState(new MoveAxisState(playerBody, moveSpeed));
    }
    private void TriggerJump()
    {
        actionSM.ChangeState(new JumpState(playerBody, jumpForce, jumpFootPoint, jumpGroundRange, jumpGroundLayer));
    }
    private void TriggerJumpWall()
    {
        if ((isFliped && inputX <= 0) || (!isFliped && inputX >= 0))
        {
            actionSM.ChangeState(new WallJumpState(playerBody, inputX * -1, 5, 5, wJPoint, wJRange, wJLayer));
        }else
            actionSM.ChangeState(new WallJumpState(playerBody, inputX, wJForceX, wJForceY, wJPoint, wJRange, wJLayer));

    }
    #endregion
    void Start()
    {
        TriggerMove();
    }
    void Update()
    {
        JumpGroundCheck();
        inputX = Input.GetAxis("Horizontal");
        if (canMove)
        {
            Flip2D();
            moveSM.ExecuteActiveState();
        }
        else if (isGround && !onStun)
        {
            canMove = true;
        }
        //
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }
    }
    private void FixedUpdate()
    {
        if (jumpRequest && isGround)
        {
            TriggerJump();
            jumpRequest = false;

        }
        else if (isWall && !isGround)
        {
            canMove = false;
            playerBody.velocity = new Vector2(0, Mathf.Clamp(playerBody.velocity.y, -wJSlideDownSpeed, float.MaxValue));
            if (jumpRequest && canWallJump)
            {
                TriggerJumpWall();
                jumpRequest = false;
                canMove = true;
            }
        }
        actionSM.ExecuteActiveState();
    }
    #region Variables
    private void JumpGroundCheck()
    {
        isGround = Physics2D.Raycast(jumpFootPoint.position, Vector2.down, jumpGroundRange, jumpGroundLayer);
        isWall = Physics2D.OverlapCircle(wJPoint.position, wJRange, wJLayer);
        canWallJump = isWall && !isGround && inputX != 0;
        
    }
    private IEnumerator ResetCanMove()
    {
        yield return new WaitForSecondsRealtime(CD);
        canMove = true;
    }

    public void Flip2D()
    {
        if ((inputX < 0 && !isFliped) || (inputX > 0 && isFliped))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); //correndo para direita;
            isFliped = !isFliped;
        }
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wJPoint.position, wJRange);
    }
}
