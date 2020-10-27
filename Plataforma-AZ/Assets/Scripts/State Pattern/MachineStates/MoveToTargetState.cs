using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoveMode { Diagonal, Horizontal, Vertical, BodyDiagonal, BodyHorizontal, BodyVertical};

public class MoveToTargetState : IStates
{
    private GameObject active;
    private Rigidbody2D activeBody;
    private Transform moveToTarget;
    private float moveToSpeed;
    private float moveToMinRange;
    private float moveToMaxRange;
    private List<Transform> moveToPoints;
    private int moveToIndex;
    private System.Action<MoveToResults> moveToResultsCallBack;
    private int moveToType = 0;
    private MoveMode moveMode;
    private bool moveToDone;

    public MoveToTargetState(GameObject active, float moveToSpeed, float moveToMinRange, float moveToMaxRange, List<Transform> moveToPoints, int moveToIndex, Action<MoveToResults> moveToResultsCallBack)
    {
        this.active = active;
        //this.moveToTarget = moveToTarget;
        this.moveToSpeed = moveToSpeed;
        this.moveToMinRange = moveToMinRange;
        this.moveToMaxRange = moveToMaxRange;
        this.moveToPoints = moveToPoints;
        this.moveToIndex = moveToIndex;
        this.moveToResultsCallBack = moveToResultsCallBack;
    }
    /// <summary>
    /// Choose the axis his can move
    /// </summary>
    /// <param name="active"></param>
    /// <param name="moveToSpeed"></param>
    /// <param name="moveToMinRange"></param>
    /// <param name="moveToMaxRange"></param>
    /// <param name="moveToPoints"></param>
    /// <param name="moveToIndex"></param>
    /// <param name="moveToResultsCallBack"></param>
    /// <param name="moveToType">0=XY, 1=X, 2=Y </param>
    public MoveToTargetState(GameObject active, float moveToSpeed, float moveToMinRange, float moveToMaxRange, List<Transform> moveToPoints, int moveToIndex, Action<MoveToResults> moveToResultsCallBack, MoveMode moveMode)
    {
        this.active = active;
        //this.moveToTarget = moveToTarget;
        this.moveToSpeed = moveToSpeed;
        this.moveToMinRange = moveToMinRange;
        this.moveToMaxRange = moveToMaxRange;
        this.moveToPoints = moveToPoints;
        this.moveToIndex = moveToIndex;
        this.moveToResultsCallBack = moveToResultsCallBack;
        this.moveMode = moveMode;
    }

    public void EnterState()
    {
        ///Debug.Log($"entrando Patroling {moveToIndex}");
        moveToTarget = moveToPoints[moveToIndex];
        moveToIndex++;
        if (moveToPoints.Count <= 0)
        {
            moveToPoints.Add(active.transform);
        }
        if (moveMode.Equals(MoveMode.BodyDiagonal) || moveMode.Equals(MoveMode.BodyVertical) || moveMode.Equals(MoveMode.BodyHorizontal))
        {
            activeBody = active.GetComponent<Rigidbody2D>();
        }
    }
    public void ExecuteState()
    {
        if (!moveToDone)
        {
            ChoseMove();
            var patrolCircleResults = new MoveToResults(moveToIndex, moveToPoints, moveToDone);
            moveToResultsCallBack(patrolCircleResults);
        }
    }
    public void ExitState()
    {
        //Debug.Log($"Saindo Patroling {moveToIndex}");
    }
    private void ChoseMove()
    {
        switch (moveMode)
        {
            case MoveMode.Diagonal:
                MoveXY();
                break;
            case MoveMode.Horizontal:
                MoveX();
                break;
            case MoveMode.Vertical:
                MoveY();
                break;
            case MoveMode.BodyDiagonal:
                MoveBodyXY();
                break;
            case MoveMode.BodyHorizontal:
                MoveBodyX();
                break;
            case MoveMode.BodyVertical:
                MoveBodyY();
                break;
            default:
                break;
        }
    }
    private void MoveXY()
    {
        if (Vector2.Distance(active.transform.position,moveToTarget.transform.position) <= moveToMinRange || Vector2.Distance(active.transform.position, moveToTarget.transform.position) >= moveToMaxRange)
        {
            // testes
            //Debug.Log("Fim da patrulha");
            //if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
            //{
            //    Debug.Log("Fora de alcance.");
            //}else
            //    Debug.Log("Dentro do alcance.");
            moveToDone = true;
        }
        else
            active.transform.position = Vector2.MoveTowards(active.transform.position, moveToTarget.position, moveToSpeed * Time.deltaTime);
    }
    private void MoveX()
    {
        if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) <= moveToMinRange || Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
        {
            moveToDone = true;
        }
        else
            active.transform.position = Vector2.MoveTowards(active.transform.position, new Vector2(moveToTarget.transform.position.x, active.transform.position.y), moveToSpeed * Time.deltaTime);
    }
    private void MoveY()
    {
        if (Mathf.Abs(active.transform.position.y - moveToTarget.position.y) <= moveToMinRange || Mathf.Abs(active.transform.position.y - moveToTarget.position.y) >= moveToMaxRange)
        {
            moveToDone = true;
        }
        else
            active.transform.position = Vector2.MoveTowards(active.transform.position, new Vector2(active.transform.position.x, moveToTarget.transform.position.y), moveToSpeed * Time.deltaTime);
    }
    // force move
    private void MoveBodyXY()
    {
        if (Vector2.Distance(active.transform.position, moveToTarget.transform.position) <= moveToMinRange || Vector2.Distance(active.transform.position, moveToTarget.transform.position) >= moveToMaxRange)
        {
            moveToDone = true;
            activeBody.velocity = Vector2.zero;
        }
        else
        {
            float speedX = active.transform.position.x >= moveToTarget.transform.position.x ? -moveToSpeed : moveToSpeed;
            float speedY = active.transform.position.y >= moveToTarget.transform.position.y ? -moveToSpeed : moveToSpeed;
            activeBody.velocity = new Vector2(speedX, speedY);
        }
    } 
    private void MoveBodyX()
    {
        if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) <= moveToMinRange || Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
        {
            moveToDone = true;
            activeBody.velocity = Vector2.zero;
        }
        else
        {
            float speedX = active.transform.position.x >= moveToTarget.transform.position.x ? -moveToSpeed : moveToSpeed;
            activeBody.velocity = new Vector2(speedX, activeBody.velocity.y);
        }
    }
    private void MoveBodyY()
    {
        if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) <= moveToMinRange || Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
        {
            moveToDone = true;
            activeBody.velocity = Vector2.zero;
        }
        else
        {
            float speedY = active.transform.position.y >= moveToTarget.transform.position.y ? -moveToSpeed : moveToSpeed;
            activeBody.velocity = new Vector2(activeBody.velocity.x, speedY);
        }
    }
}

public class MoveToResults
{
    public int activeMoveToPoint;
    public List<Transform> allMoveToPoints;
    public bool patrolDone;
    public MoveToResults(int activePatrolPoint, List<Transform> allPatrolPoints, bool patrolDone)
    {
        this.activeMoveToPoint = activePatrolPoint;
        this.allMoveToPoints = allPatrolPoints;
        this.patrolDone = patrolDone;
    }
}
