using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    public StateMachine moveSM = new StateMachine();
    public StateMachine actionSM = new StateMachine();
    public Rigidbody2D playerBody;
    public float moveSpeed;
    public Vector2 moveAxis;
    public bool canMove;

    void Start()
    {
        moveSM.ChangeState(new MoveAxisState(playerBody, moveSpeed, canMove));
        //actionSM.ChangeState(new MoveAxisState(playerBody, moveSpeed, canMove));
    }

    void Update()
    {
        moveSM.ExecuteActiveState();
        actionSM.ExecuteActiveState();
    }

    public void TriggerMoveAxis()
    {
        //actionSM.ChangeState(new MoveToTargetState());
    }
}
