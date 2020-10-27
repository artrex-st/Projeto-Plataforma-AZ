using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSM : MonoBehaviour, ICombat
{
    public StateMachine moveSM = new StateMachine();
    public StateMachine actionSM = new StateMachine();
    [Header("Player Components")]
    public Animator playerAnimator;
    public Rigidbody2D playerBody;
    public SpriteRenderer playerSprite;
    public UiBar healthBar;
    public TextMeshProUGUI textmeshPro;
    public float playerHP;
    public float playerMaxHP;
    public int playerKeys;

    [Header("Timers")]
    public float resumeControlCD;

    [Header("Boolean's")]
    public bool canMove;
    public bool isFliped;
    public bool jumpRequest;
    public bool isGround;
    public bool isWall;
    public bool canWallJump;
    public bool onStun;
    public bool onGroundSlide;
    public bool onHurt; //

    [Header("Move")]
    public float moveSpeed;
    public float inputX;
    public float inputY;

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

    [Header("Ground Slide")]
    // playerBody
    // InputX
    public float gSSpeed;
    public float gSTime;
    public Collider2D gSBaseColl;
    public Collider2D gSNewColl;

    [Header("Testes")]
    public float CD;
    public bool test;

    #region States triggrers
    private void TriggerMove()
    {
        onGroundSlide = false;
        moveSM.ChangeState(new MoveAxisState(playerBody, moveSpeed));
    }
    private void TriggerMoveSlide()
    {
        if (inputX >= 0.8f && playerBody.velocity.x >= 0.8f)
        {
            onGroundSlide = true;
            moveSM.ChangeState(new MoveSlideState(playerBody, 1, gSSpeed, gSBaseColl, gSNewColl, gSTime));
        }
        else if (inputX <= -0.8f && playerBody.velocity.x <= -0.8f)
        {
            onGroundSlide = true;
            moveSM.ChangeState(new MoveSlideState(playerBody, -1, gSSpeed, gSBaseColl, gSNewColl, gSTime));
        }
    }
    private void TriggerJump()
    {
        actionSM.ChangeState(new JumpState(playerBody, jumpForce, jumpFootPoint, jumpGroundRange, jumpGroundLayer));
    }
    private void TriggerJumpWall()
    {
        if ((isFliped && Input.GetAxisRaw("Horizontal") <= 0) || (!isFliped && Input.GetAxisRaw("Horizontal") >= 0))
        {
            actionSM.ChangeState(new WallJumpState(playerBody, inputX * -1, 0, 1, wJPoint, wJRange, wJLayer));
        }else
            actionSM.ChangeState(new WallJumpState(playerBody, inputX, wJForceX, wJForceY, wJPoint, wJRange, wJLayer));
    }
    #endregion
    void Start()
    {
        playerHP = playerMaxHP;
        TriggerMove();
        healthBar.SetMaxFill(playerMaxHP);
        textmeshPro.text = $"{textmeshPro.text} \n Versão:{Application.version}";
    }
    void Update()
    {
        UiKeys();
        JumpGroundCheck();
        inputX = Input.GetAxis("Horizontal");
        //
        if (Input.GetAxisRaw("Vertical") == -1 && Mathf.Abs(playerBody.velocity.x) >= 0.1 && isGround && !isWall)
        {
            if (!onGroundSlide)
            {
                TriggerMoveSlide();
            }
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            TriggerMove();
        }
        //
        if (canMove)
        {
            if (!onGroundSlide)
            {
                Flip2D();
            }
            moveSM.ExecuteActiveState();
        }
        else if (isGround && !onStun || playerBody.velocity.y < -5)
        {
            canMove = true;
        }
        //
        if (Input.GetButtonDown("Jump") && (isGround || canWallJump))
        {
            jumpRequest = true;
        }
        Animations();
    }
    private void FixedUpdate()
    {
        if (jumpRequest && isGround && !canWallJump)
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
    private void Animations()
    {
        playerAnimator.SetFloat("Move", Mathf.Abs(inputX));
        playerAnimator.SetFloat("JumpSpeed", playerBody.velocity.y);
        playerAnimator.SetBool("isGround", isGround);
        playerAnimator.SetBool("isWall", isWall);
        playerAnimator.SetBool("isGroundSlide", onGroundSlide);
        
    }
    private void UiKeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void JumpGroundCheck()
    {
        isGround = Physics2D.Raycast(jumpFootPoint.position, Vector2.down, jumpGroundRange, jumpGroundLayer);
        isWall = Physics2D.OverlapCircle(wJPoint.position, wJRange, wJLayer);
        canWallJump = isWall && !isGround && inputX != 0 && playerBody.velocity.y <= 0;
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
    public void ApplyDmg(float dmg)
    {
        if (!onHurt)
        {
            playerHP -= dmg;
            healthBar.SetFill(playerHP);
            onHurt = true;
            StartCoroutine(ResumeControl());
        }
    }
    private IEnumerator ResumeControl()
    {
        if (playerSprite.color == Color.white)
        {
            playerSprite.color = Color.Lerp(Color.white, Color.black, 5 * Time.deltaTime);
        }else
            playerSprite.color = Color.Lerp(Color.white, Color.black, 5 * Time.deltaTime);


        yield return new WaitForSeconds(resumeControlCD);
        onHurt = false;

    }
    public void ApplyDmg(float dmg, string type)
    {
        throw new System.NotImplementedException();
    }
    public void AddKeys(int keyValue)
    {
        playerKeys += keyValue;
    }
    public void UseKeys(int keyValue)
    {
        playerKeys -= keyValue;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("CenarioMove") && playerBody.velocity == Vector2.zero)
        {
            transform.parent = collision.transform;
        }
        else
            transform.parent = null;
    }

    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wJPoint.position, wJRange);
    }

}
