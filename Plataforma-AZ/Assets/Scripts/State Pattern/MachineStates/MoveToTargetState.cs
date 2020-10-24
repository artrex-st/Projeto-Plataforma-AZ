using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetState : IStates
{
    private GameObject active;
    private Transform moveToTarget;
    private float moveToSpeed;
    private float moveToMinRange;
    private float moveToMaxRange;
    private List<Transform> moveToPoints;
    private int moveToIndex;
    private System.Action<MoveToResults> moveToResultsCallBack;
    private int moveToType = 0;

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
    public MoveToTargetState(GameObject active, float moveToSpeed, float moveToMinRange, float moveToMaxRange, List<Transform> moveToPoints, int moveToIndex, Action<MoveToResults> moveToResultsCallBack, int moveToType)
    {
        this.active = active;
        //this.moveToTarget = moveToTarget;
        this.moveToSpeed = moveToSpeed;
        this.moveToMinRange = moveToMinRange;
        this.moveToMaxRange = moveToMaxRange;
        this.moveToPoints = moveToPoints;
        this.moveToIndex = moveToIndex;
        this.moveToResultsCallBack = moveToResultsCallBack;
        this.moveToType = moveToType;
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
        if (moveToType == 0)
        {
            MoveXY();
        }
        else if (moveToType == 1)
        {
            MoveX();
        }
        else if (moveToType == 2)
        {
            MoveY();
        }
    }
    private void MoveXY()
    {
        if (Vector2.Distance(active.transform.position,moveToTarget.transform.position) <= moveToMinRange || Vector2.Distance(active.transform.position, moveToTarget.transform.position) >= moveToMaxRange)
        {
            moveToDone = true;
        }else
            active.transform.position = Vector2.MoveTowards(active.transform.position, moveToTarget.position, moveToSpeed * Time.deltaTime);
    }
    private void MoveX()
    {
        if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) <= moveToMinRange || Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
        {
            moveToDone = true;
            // testes
            //Debug.Log("Fim da patrulha");
            //if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
            //{
            //    Debug.Log("Fora de alcance.");
            //}else
            //    Debug.Log("Dentro do alcance.");
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
