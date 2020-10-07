using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;
using Unity.Mathematics;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour, ICombat
{

    // Base States
    [Tooltip("Max Health value.")]
    public float maxHP = 100;
    [Tooltip("Current Health value.")]
    public float currHP;
    [Range(0f, 50f), Tooltip("Speed of player will moved.")]
    public float speed = 10;
    [Range(-30f, 30f), Tooltip("Axis of movement direction.")]
    public float moveInX, moveInY;
    // Status of Features
    [Space(10), Range(0f, 50f), Tooltip("The more jumpforce it has, more higher and faster it will go.")]
    public float jumpForce = 20;
    [Range(0f, 50f), Tooltip("Gravity Scale of Player body.")]
    public float gravityScale = 5;
    [Range(0f, 10f), Tooltip("Divisor of Gravity Scale for wall Slide.")]
    public float wallSlide;
    public float flipingCd;
    [Space(10)]
    public float keys=0;
    public float stumTime;

    public static bool isEdgeR, isEdgeL, isWallEdge, isGround, isIce, isFliping, isPunching, isKicking, isGroundSlide;
    public static bool canWS = true, canFlip = true, canMove = true;
    public bool stun;
    [Space(10)]
    public float ecoSpeed;
    [Tooltip("testes")]
    public bool visEdgeR, visEdgeL, visWallEdge, visGround, visIce, visGroundSlide;
    public Transform footPosition;
    public LayerMask layerOfGround;
    public Rigidbody2D rbPlayer;
    private Animator aniPlayer;


    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        aniPlayer = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        visEdgeR = isEdgeR;
        visEdgeL = isEdgeL;
        visWallEdge = isWallEdge;
        visGround = isGround;
        isIce = visIce;
        visGroundSlide = isGroundSlide;
        rbPlayer.gravityScale = gravityScale;
        moveInX = Input.GetAxis("Horizontal");
        moveInY = Input.GetAxis("Vertical");
        EdgeCheck();
        AnimationsCheck();
        canMove = !stun;
        if (stun)
        {
            StartCoroutine(EndStum());
        }

    }
    IEnumerator EndStum()
    {
        yield return new WaitForSecondsRealtime(stumTime);
        stun = false;
    }
    private void EdgeCheck()
    {
        isGround = Physics2D.OverlapBox(footPosition.position + new Vector3(0, -0.03f), new Vector2(0.8f, 0.07f), 0, layerOfGround);

        isEdgeL = !Physics2D.OverlapBox(transform.position + new Vector3(0.4f, -0.28f), new Vector2(0.46f, 2.2f), 0, layerOfGround);
        isEdgeR = !Physics2D.OverlapBox(transform.position - new Vector3(0.4f, 0.28f), new Vector2(0.46f, 2.2f), 0, layerOfGround);

        isWallEdge = Physics2D.OverlapBox(transform.position, new Vector2(1.3f, 0.2f), 0, layerOfGround);
    }
    private void AnimationsCheck()
    {
        //animation
        aniPlayer.SetBool("IsIce",isIce);
        aniPlayer.SetBool("IsGroundSlide", isGroundSlide);
        aniPlayer.SetBool("IsFliping", isFliping); // wall jump
        aniPlayer.SetBool("IsPunching", isPunching); // attack (soco)
        aniPlayer.SetBool("IsKicking", isKicking); // attack (chute)
        if (canMove)
        {
            if (math.abs(rbPlayer.velocity.x) >= 0.1f)
            {
                aniPlayer.SetFloat("Run", math.abs(moveInX));
            }else
                aniPlayer.SetFloat("Run", 0);

            aniPlayer.SetFloat("SpeedX", math.abs(ecoSpeed));
        }
        if (moveInX > 0.01f && !isWallEdge && !isGroundSlide)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false; //correndo para direita;
        }
        else
        if (moveInX < -0.01f && !isWallEdge && !isGroundSlide)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        }
        else
        if (isEdgeL || isEdgeR && isGround)
        {
            if (isEdgeL && !isEdgeR && rbPlayer.velocity.y <= 0)
            {
                aniPlayer.SetBool("IsBalance", true);
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else
            if (isEdgeR && !isEdgeL && rbPlayer.velocity.y <= 0)
            {
                aniPlayer.SetBool("IsBalance", true);
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
        }
        else
            aniPlayer.SetBool("IsBalance", false);
        
        aniPlayer.SetFloat("JumpForce", rbPlayer.velocity.y);
        aniPlayer.SetBool("IsGround", isGround);

        if (rbPlayer.velocity.y <= 0 && canWS)
        {
            WallSlideCheck();
        }

    }
    private void WallSlideCheck()
    {
        if (isEdgeL && !isEdgeR && !isGround && rbPlayer.velocity.y <= 0)
        {
            aniPlayer.SetBool("Wall.Slide", true);
            GetComponentInChildren<SpriteRenderer>().flipX = false;

            rbPlayer.gravityScale = gravityScale / wallSlide;
        }
        else
        if (isEdgeR && !isEdgeL && !isGround && rbPlayer.velocity.y <= 0)
        {
            aniPlayer.SetBool("Wall.Slide", true);
            GetComponentInChildren<SpriteRenderer>().flipX = true;

            rbPlayer.gravityScale = gravityScale / wallSlide;
        }
        else
        {
            aniPlayer.SetBool("Wall.Slide", false);
            rbPlayer.gravityScale = gravityScale;
        }
    }
    public void UseKey()
    {
        keys--;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.4f, -0.28f), new Vector3(0.36f, 2.2f, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - new Vector3(0.4f, 0.28f), new Vector3(0.36f, 2.2f, 1));
    }

    public void ApplyDmg(float dmg)
    {
        currHP -= dmg;
    }

    public void ApplyDmg(float dmg, string type)
    {
        throw new System.NotImplementedException();
    }
}
