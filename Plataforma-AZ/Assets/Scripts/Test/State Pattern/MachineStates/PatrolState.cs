using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IStates
{
    private GameObject active;
    private Transform patrolTarget;
    private float patrolSpeed;
    private float patrolMinRange;
    private float patrolMaxRange;
    private List<Transform> patrolPoints;
    private int patrolIndex;
    private System.Action<PatrolResults> patrolResultsCallBack;

    private bool patrolDone;

    public PatrolState(GameObject active, Transform patrolTarget, float patrolSpeed, float patrolMinRange, float patrolMaxRange, List<Transform> patrolPoints, int patrolIndex, Action<PatrolResults> patrolResultsCallBack)
    {
        this.active = active;
        this.patrolTarget = patrolTarget;
        this.patrolSpeed = patrolSpeed;
        this.patrolMinRange = patrolMinRange;
        this.patrolMaxRange = patrolMaxRange;
        this.patrolPoints = patrolPoints;
        this.patrolIndex = patrolIndex;
        this.patrolResultsCallBack = patrolResultsCallBack;
    }

    public void EnterState()
    {
        Debug.Log($"entrando Patroling {patrolIndex}");
        patrolIndex++;
        if (patrolPoints.Count <= 0)
        {
            patrolPoints.Add(active.transform);
        }
    }

    public void ExecuteState()
    {
        if (!patrolDone)
        {
            Debug.Log($"Patroling {patrolIndex} disctancia: {Mathf.Abs(active.transform.position.x - patrolTarget.position.x)}");
            active.transform.position = Vector2.MoveTowards(active.transform.position,patrolTarget.position,patrolSpeed * Time.deltaTime);
            if (Mathf.Abs(active.transform.position.x - patrolTarget.position.x) <= patrolMinRange || Mathf.Abs(active.transform.position.x - patrolTarget.position.x) >= patrolMaxRange)
            {
                patrolDone = true;
                // testes
                Debug.Log("Fim da patrulha");
                if (Mathf.Abs(active.transform.position.x - patrolTarget.position.x) >= patrolMaxRange)
                {
                    Debug.Log("Fora de alcance.");
                }else
                    Debug.Log("Dentro do alcance.");
            }
            var patrolCircleResults = new PatrolResults(patrolIndex, patrolPoints, patrolDone);
            patrolResultsCallBack(patrolCircleResults);
        }
    }

    public void ExitState()
    {
        Debug.Log($"Saindo Patroling {patrolIndex}");
    }
}

public class PatrolResults
{
    public int activePatrolPoint;
    public List<Transform> allPatrolPoints;
    public bool patrolDone;
    public PatrolResults(int activePatrolPoint, List<Transform> allPatrolPoints, bool patrolDone)
    {
        this.activePatrolPoint = activePatrolPoint;
        this.allPatrolPoints = allPatrolPoints;
        this.patrolDone = patrolDone;
    }
}
