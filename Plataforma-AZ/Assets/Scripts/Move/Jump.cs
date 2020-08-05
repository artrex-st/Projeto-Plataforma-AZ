using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;
using System;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private Transform footPosition;
    private LayerMask layerOfGround;
    private Boolean jumpWithTime = false,jumpRequest = false, wallJumpRequest= false;
    private float jumpForce;

    private void Start()
    {
        footPosition = GetComponent<PlayerController>().footPosition;
        layerOfGround = GetComponent<PlayerController>().layerOfGround;
        jumpForce = GetComponent<PlayerController>().jumpForce;
    }

    private void Update()
    {
        //teste
        Start();
        //
        if (Input.GetButtonDown("Jump") && Check.FGrounded(footPosition, layerOfGround))
        {
            jumpRequest = true;
        }
        if (Input.GetButtonDown("Jump") && !PlayerController.isGround && PlayerController.isWallEdge)
        {
            wallJumpRequest = true;
            GetComponentInChildren<Animator>().SetTrigger("Jump");
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<PlayerController>().moveInX * 20, jumpForce / GetComponent<PlayerController>().wallSlide), ForceMode2D.Impulse);
            Debug.Log($"Wall Jump {GetComponent<PlayerController>().speed}");
            wallJumpRequest = false;
        }
    }
}
