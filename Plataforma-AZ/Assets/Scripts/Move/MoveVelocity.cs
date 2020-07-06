using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using ArtrexUtils;

//requerimentos minimos
//Minimun Requirements
[RequireComponent(typeof(Rigidbody2D))]

public class MoveVelocity : MonoBehaviour, IMove
{
    private Vector2 velocityVector;
    private Rigidbody2D rb2D;
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
        rb2D.velocity = new Vector2(velocityVector.x * GetComponent<PlayerController>().speed, rb2D.velocity.y);
        GetComponent<Animator>().SetFloat("Run", math.abs(velocityVector.x));

        transform.rotation = Check.Flip2D(velocityVector.x,transform.rotation);
    }
}
