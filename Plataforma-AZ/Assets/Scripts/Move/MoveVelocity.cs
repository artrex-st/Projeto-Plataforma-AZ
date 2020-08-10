using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;

//requerimentos minimos
//Minimun Requirements
[RequireComponent(typeof(Rigidbody2D))]

public class MoveVelocity : MonoBehaviour, IMove
{
    private Vector2 velocityVector;
    private Rigidbody2D rb2D;
    [SerializeField]
    private float speed;
    //private Character_Base characterBase;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector2 velocityVector)
    {
        this.velocityVector = velocityVector;
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
        rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
    }
    private void AirMove()
    {
        //speed = Mathf.Clamp(velocityVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
        rb2D.velocity = new Vector2(GetComponent<PlayerController>().speed * velocityVector.x / 2, rb2D.velocity.y);
        EcoMoveReset(8);
    }
    private void WallToFlip()
    {
        rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed * 1.5f, rb2D.velocity.y);
        EcoMoveReset(8);
    }
    private void IceMove() //teste
    {
        if (PlayerController.isWallEdge)
        {
            rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
            speed = 0;
        }
        else
        {
            speed = Mathf.Clamp(velocityVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }
        EcoMoveReset(2);
    }
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
