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
        if (PlayerController.isGround && !PlayerController.isIce)
        {
            NormalMove();
        }else
        if (PlayerController.isGround && PlayerController.isIce)
        {
            IceMove();
        }
        else
        if (!PlayerController.isGround && !PlayerController.isWallEdge)
        {
            AirMove();
        }else
        if (!PlayerController.isGround && PlayerController.isWallEdge)
        {
            WallMove();
        }
    }
    private void NormalMove()
    {
        rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
    }
    private void AirMove()
    {
        speed = Mathf.Clamp(velocityVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        if (velocityVector.x == 0 || PlayerController.isGround)
        {
            speed = 0;
        }
    }
    private void WallMove()
    {
        rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed / 2f, rb2D.velocity.y);
    }
    private void IceMove() //teste
    {
        speed = Mathf.Clamp(velocityVector.x + speed, -GetComponent<PlayerController>().speed, GetComponent<PlayerController>().speed);
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        if (speed >= 0.1f)
        {
            speed -= Time.deltaTime;
        }else
        if (speed <= -0.1f)
        {
            speed += Time.deltaTime;
        }
    }
}
