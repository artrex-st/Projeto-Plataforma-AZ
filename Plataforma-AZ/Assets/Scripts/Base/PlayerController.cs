using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtrexUtils;

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

    public Transform footPosition;
    public LayerMask layerOfGround;
    public Rigidbody2D rbPlayer;


    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rbPlayer.gravityScale = gravityScale;
        moveInX = GetComponent<Rigidbody2D>().velocity.x;
    }
}
