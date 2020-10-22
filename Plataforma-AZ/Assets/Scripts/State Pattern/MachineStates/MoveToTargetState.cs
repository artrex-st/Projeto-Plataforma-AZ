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

    private bool moveToDone;

    public MoveToTargetState(GameObject active, Transform moveToTarget, float moveToSpeed, float moveToMinRange, float moveToMaxRange, List<Transform> moveToPoints, int moveToIndex, Action<MoveToResults> moveToResultsCallBack)
    {
        this.active = active;
        this.moveToTarget = moveToTarget;
        this.moveToSpeed = moveToSpeed;
        this.moveToMinRange = moveToMinRange;
        this.moveToMaxRange = moveToMaxRange;
        this.moveToPoints = moveToPoints;
        this.moveToIndex = moveToIndex;
        this.moveToResultsCallBack = moveToResultsCallBack;
    }

    public void EnterState()
    {
        Debug.Log($"entrando Patroling {moveToIndex}");
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
            Debug.Log($"Patroling {moveToIndex} disctancia: {Mathf.Abs(active.transform.position.x - moveToTarget.position.x)}");
            active.transform.position = Vector2.MoveTowards(active.transform.position,moveToTarget.position,moveToSpeed * Time.deltaTime);
            if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) <= moveToMinRange || Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
            {
                moveToDone = true;
                // testes
                Debug.Log("Fim da patrulha");
                if (Mathf.Abs(active.transform.position.x - moveToTarget.position.x) >= moveToMaxRange)
                {
                    Debug.Log("Fora de alcance.");
                }else
                    Debug.Log("Dentro do alcance.");
            }
            var patrolCircleResults = new MoveToResults(moveToIndex, moveToPoints, moveToDone);
            moveToResultsCallBack(patrolCircleResults);
        }
    }

    public void ExitState()
    {
        Debug.Log($"Saindo Patroling {moveToIndex}");
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
