using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;
using System;

public class Jump : MonoBehaviour
{
    private Transform footPosition;
    private LayerMask layerOfGround;
    private Boolean jumpWithTime = false;
    private Boolean jumpRequest = false;
    private float jumpForce;

    private void Start()
    {
        footPosition = GetComponent<PlayerController>().footPosition;
        layerOfGround = GetComponent<PlayerController>().layerOfGround;
        jumpForce = GetComponent<PlayerController>().jumpForce;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && Check.FGrounded(footPosition, layerOfGround))
        {
            jumpRequest = true;
            GetComponentInChildren<Animator>().SetTrigger("Jump");
        }

        GetComponentInChildren<Animator>().SetFloat("JumpForce", GetComponent<Rigidbody2D>().velocity.y);
        GetComponentInChildren<Animator>().SetBool("IsGround", Check.FGrounded(footPosition, layerOfGround));
    }

    void FixedUpdate()
    {
        if (jumpRequest)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }
}
