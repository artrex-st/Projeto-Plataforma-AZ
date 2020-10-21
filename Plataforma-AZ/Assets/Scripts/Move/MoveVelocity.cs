using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;
using System;
using Unity.Mathematics;

//requerimentos minimos
//Minimun Requirements
[RequireComponent(typeof(Rigidbody2D))]

public class MoveVelocity : MonoBehaviour, IMove
{
    private Vector2 axisVector;
    private Rigidbody2D rb2D;
    [SerializeField]
    private float speed, groundSpeed, groundSlideTimer;
    [SerializeField]
    private Boolean downRequest;
    //private Character_Base characterBase;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector2 velocityVector)
    {
        this.axisVector = velocityVector;
    }
    public void Update()
    {
        if (axisVector.y <= -0.1 && !downRequest && math.abs(axisVector.x) >= 0.05f)
        {
            downRequest = true;
        }else
        if (axisVector.y >= 0 && !PlayerController.isFliping)
        {
            downRequest = false;
        }
    }
    private void FixedUpdate()
    {
        GetComponent<PlayerController>().ecoSpeed = speed;
        if (PlayerController.canMove)
        {
            if (PlayerController.isGround && !PlayerController.isIce)
            {
                NormalMove();
                //Debug.Log("normal");
            }
            else
            if (PlayerController.isGround && PlayerController.isIce)
            {
                IceMove();
                //Debug.Log("Ice");
            }
            else
            if (!PlayerController.isGround && !PlayerController.isWallEdge && !PlayerController.isFliping)
            {
                AirMove();
                //Debug.Log("Air");
            }
            else
            if (!PlayerController.isGround && PlayerController.isFliping)
            {
                WallToFlip();
                //Debug.Log("Wall to Flip");
            }
        }
        else
            StopingMove();
    }
    private void NormalMove()
    {
        if (!downRequest)
        {
            rb2D.velocity = new Vector2(axisVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
            PlayerController.isGroundSlide = false;
        }else
        if (downRequest)
        {
            GroundSliding();
        }
    }
    private void IceMove() //teste
    {
        if (PlayerController.isWallEdge)
        {
            rb2D.velocity = new Vector2(axisVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
            speed = 0;
        }
        else
        {
            if (!downRequest)
            {
                speed = Mathf.Clamp(axisVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
                PlayerController.isGroundSlide = false;
            }
            else
            if (downRequest)
            {
                GroundIceSliding();
            }
        }
        EcoMoveReset(2);
    }
    private void AirMove()
    {
        //speed = Mathf.Clamp(velocityVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
        rb2D.velocity = new Vector2(GetComponent<PlayerController>().speed * axisVector.x, rb2D.velocity.y);
        EcoMoveReset(8);
    }
    private void WallToFlip()
    {
        rb2D.velocity = new Vector2(axisVector.x * GetComponent<PlayerController>().speed * 1.5f, rb2D.velocity.y);
        EcoMoveReset(8);
    }

    // ground slidings
    private void GroundSliding() //teste
    {
        Debug.Log($"Down request:{downRequest}");
        rb2D.velocity = new Vector2(rb2D.velocity.x > 0 ? groundSpeed - groundSlideTimer : -groundSpeed + groundSlideTimer, rb2D.velocity.y);
        PlayerController.isGroundSlide = true;
        if (Mathf.Abs(rb2D.velocity.x) <= 0.06f || !PlayerController.isGround || Input.GetAxis("Vertical") >=0)
        {
            PlayerController.isGroundSlide = false;
        }
    }
    private void GroundIceSliding() //teste
    {
        Debug.Log($"Down request:{downRequest}");
        rb2D.velocity = new Vector2(speed > 0 ? groundSpeed - groundSlideTimer : -groundSpeed + groundSlideTimer, rb2D.velocity.y);
        PlayerController.isGroundSlide = true;
        if (Mathf.Abs(rb2D.velocity.x) <= 0.06f || !PlayerController.isGround || Input.GetAxis("Vertical") >= 0)
        {
            PlayerController.isGroundSlide = false;
        }
    }
    // ground end
    private void StopingMove() //teste
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }
    private void EcoMoveReset(float multiply)
    {
        if (speed >= 0.1f)
        {
            speed -= 0.02f * multiply;
        }
        else
        if (speed <= -0.01f)
        {
            speed += 0.02f * multiply;
        }
    }
}
