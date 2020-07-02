using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//requerimentos minimos
//Minimun Requirements
[RequireComponent(typeof(Rigidbody2D))]

public class MoveVelocity : MonoBehaviour, IMove
{
    [SerializeField] 
    private float moveSpeed = 10;
    private Vector2 velocityVector;
    private Rigidbody2D rigidbody2D;
    //private Character_Base characterBase;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        //characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector2 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(velocityVector.x * moveSpeed, rigidbody2D.velocity.y);
        //characterBase.PlayMoveAinm(velocityVector);
    }
}
