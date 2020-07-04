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
    public float moveTo;
    // Status of Features
    [Space(10), Range(0f, 50f), Tooltip("The more jumpforce it has, more higher and faster it will go.")]
    public float jumpForce = 20;
    [Range(0f, 10f), Tooltip("Building...")]
    public float gravity = 2;
    [Range(0f, 100f), Tooltip("How much fuel his can store.")]
    public float maxFuel = 50;
    [Range(0f, 100f), Tooltip("How much fuel his has. (need fuel to fly)")]
    public float fuel = 50;
    [Tooltip("Fuel cost per Second.")]
    public float fuelCost;
    [Range(0f, 50f), Tooltip("The higher is the value, more faster it will goes to up")]
    public float flyForce = 40f;
    // Status of Combat
    [Space(10), Range(-100f, 100f), Tooltip("Addes more Damage to attacks of player.")]
    public float attack;
    [Range(-100f, 100f), Tooltip("Reduce damage taken of enemies.")]
    public float defence;
    [Range(-100f, 100f), Tooltip("Building...")]
    public float resist;
    [Range(-100f, 100f), Tooltip("Building...")]
    public float breath;
    [Range(-10f, 10f), Tooltip("Time to use next Attack.")]
    public float WeaponCooldown;
    // Check's
    [Space(10), Tooltip("Indicate if Player Can Move.")]
    public bool canMove;
    [Tooltip("Indicates if Player has moving.")]
    public bool isMoving;
    [Tooltip("Indicates if Player has touche the ground layers.")]
    public bool isGround;
    [Tooltip("Indicates if Player has Crouch.")]
    public bool isCrouch;
    [Tooltip("Indicates if Player has Falling")]
    public bool isFalling;
    [Tooltip("Building...")]
    public bool isWall;
    [Tooltip("Indicates if Player can Fly.")]
    public bool fly;
    // action
    [Tooltip("Keys")]
    public int keys = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
