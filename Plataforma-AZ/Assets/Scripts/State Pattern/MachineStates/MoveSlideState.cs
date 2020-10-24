using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSlideState : IStates
{
    [Header("Ground Slide")]
    private Rigidbody2D active;
    private float inputX;
    private float gSSpeed;
    private Collider2D gSBaseColl;
    private Collider2D gSNewColl;
    private bool gSDone;
    private float gSTime;

    public MoveSlideState(Rigidbody2D active, float inputX, float gSSpeed, Collider2D gSBaseColl, Collider2D gSNewColl, float gSTime)
    {
        this.active = active;
        this.inputX = inputX;
        this.gSSpeed = gSSpeed;
        this.gSBaseColl = gSBaseColl;
        this.gSNewColl = gSNewColl;
        this.gSTime = gSTime;
    }
    public void EnterState()
    {
        gSDone = false;
        gSBaseColl.enabled = false;
        gSNewColl.enabled = true;
    }
    public void ExecuteState()
    {
        inputX = Mathf.Lerp(inputX, 0, gSTime * Time.deltaTime);
        active.velocity = new Vector2(inputX * gSSpeed, active.velocity.y);
    }
    public void ExitState()
    {
        gSBaseColl.enabled = true;
        gSNewColl.enabled = false;
    }
}
