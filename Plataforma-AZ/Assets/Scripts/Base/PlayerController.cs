using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{

    // Base States
    [Tooltip("Max Health value.")]
    public float maxHP = 100;
    [Tooltip("Current Health value.")]
    public float currHP;
    [Range(0f, 50f), Tooltip("Speed of player will moved.")]
    public float speed = 10;
    [Range(-30f, 30f), Tooltip("Axis of movement direction.")]
    public float moveInX;
    // Status of Features
    [Space(10), Range(0f, 50f), Tooltip("The more jumpforce it has, more higher and faster it will go.")]
    public float jumpForce = 20;
    [Range(0f, 50f), Tooltip("Gravity Scale of Player body.")]
    public float gravityScale = 5;
    public bool isEdgeR, isEdgeL, isWallEdge, isGround;
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
        rbPlayer.gravityScale = gravityScale;
        moveInX = Input.GetAxis("Horizontal");
        EdgeCheck();
        AnimationsCheck();
        
    }

    private void EdgeCheck()
    {
        isGround = Physics2D.OverlapBox(footPosition.position + new Vector3(0, -0.03f), new Vector2(0.8f, 0.07f), 0, layerOfGround);
        
        isEdgeL = !Physics2D.OverlapBox(transform.position + new Vector3(0.5f, -0.28f), new Vector2(0.1f, 2.2f), 0, layerOfGround);
        isEdgeR = !Physics2D.OverlapBox(transform.position - new Vector3(0.5f, 0.28f), new Vector2(0.1f, 2.2f), 0, layerOfGround);

        isWallEdge = Physics2D.OverlapBox(transform.position, new Vector2(1.3f, 0.2f), 0, layerOfGround);
    }
    private void AnimationsCheck()
    {
        //animation
        aniPlayer.SetFloat("Run", math.abs(rbPlayer.velocity.x));
        if (rbPlayer.velocity.x > 0 && !isWallEdge)
            GetComponentInChildren<SpriteRenderer>().flipX = false; //correndo para direita;
        else
            if (rbPlayer.velocity.x < 0 && !isWallEdge)
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        aniPlayer.SetFloat("JumpForce", rbPlayer.velocity.y);
        aniPlayer.SetBool("IsGround", isGround);
        if (isWallEdge)
        {
            if (isEdgeL)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }else
                GetComponentInChildren<SpriteRenderer>().flipX = false;

            aniPlayer.SetBool("Wall.Slide",isWallEdge);
        }

    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, -0.28f), new Vector3(0.1f, 2.2f, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - new Vector3(0.5f, 0.28f), new Vector3(0.1f, 2.2f, 1));
    }
}
