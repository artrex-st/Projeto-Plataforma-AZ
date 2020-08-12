using ArtrexUtils;
using System;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private Transform footPosition;
    private LayerMask layerOfGround;
    private Boolean jumpGravity = false,jumpRequest = false, wallJumpRequest= false;
    [SerializeField]
    private float jumpForce, flipingCd, flipingTime;

    private void Start()
    {
        footPosition = GetComponent<PlayerController>().footPosition;
        layerOfGround = GetComponent<PlayerController>().layerOfGround;
        jumpForce = GetComponent<PlayerController>().jumpForce;
        flipingCd = GetComponent<PlayerController>().flipingCd;
    }

    private void Update()
    {
        //teste
        Start();
        //
        if (Input.GetButtonDown("Jump") && Check.FGrounded(footPosition, layerOfGround))
        {
            jumpRequest = true;
            PlayerController.isGroundSlide = false;
        }
        if (Input.GetButtonDown("Jump") && !PlayerController.isGround && PlayerController.isWallEdge && flipingTime >= flipingCd && PlayerController.canFlip)
        {
            wallJumpRequest = true;
            PlayerController.isGroundSlide = false;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    //Gizmo do isGround
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(footPosition.position + new Vector3(0, -0.03f), new Vector2(0.8f, 0.07f));
    //}
    void FixedUpdate()
    {
        if (jumpRequest)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }
        if (wallJumpRequest)
        {
            if (PlayerController.isEdgeL && GetComponent<PlayerController>().moveInX > 0)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<PlayerController>().moveInX * jumpForce, jumpForce), ForceMode2D.Impulse);
                PlayerController.isFliping = true;
                flipingTime = 0;
            }else
            if (PlayerController.isEdgeR && GetComponent<PlayerController>().moveInX < 0)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<PlayerController>().moveInX * jumpForce, jumpForce), ForceMode2D.Impulse);
                PlayerController.isFliping = true;
                flipingTime = 0;
            }            
            wallJumpRequest = false;
        }
        flipingTime += Time.deltaTime;
        if (PlayerController.isFliping && flipingTime >=1 || PlayerController.isGround)
        {
            PlayerController.isFliping = false;
        }
    }
}
