using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;

public class JumpWithTime : MonoBehaviour
{
    [SerializeField]
    public bool isGrounded;
    private Rigidbody2D rbJump;
    public Transform footPosition;
    public LayerMask layerOfGround;

    private void Start()
    {
        rbJump = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && Check.FGrounded(footPosition, layerOfGround))
        {
            rbJump.AddForce(Vector2.up * GetComponent<PlayerController>().jumpForce, ForceMode2D.Impulse);

        }
    }
}
