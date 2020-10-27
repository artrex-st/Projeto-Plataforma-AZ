using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlataform : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    public GameObject active;
    //public enum MoveMode { Diagonal, Horizontal, Vertical}
    public MoveMode moveMode = MoveMode.Diagonal;
    [Header("Patrol")]
    public Transform moveToTarget;
    public float moveToSpeed;
    public float moveToMinRange;
    public float moveToMaxRange;
    public List<Transform> moveToPoints;
    public int moveToIndex;
    public float moveDelay;

    void Start()
    {
        TriggerMoveTo();
    }

    void Update()
    {
        stateMachine.ExecuteActiveState();
    }

    #region trigger
    private void TriggerMoveTo()
    {
        stateMachine.ChangeState(new MoveToTargetState(active, moveToSpeed, moveToMinRange, moveToMaxRange, moveToPoints, moveToIndex, MoveToDone, moveMode));
    }
    #endregion

    public void MoveToDone(MoveToResults moveToResult)
    {
        if (moveToIndex != moveToResult.activeMoveToPoint && moveToResult.patrolDone)
        {
            moveToIndex = moveToResult.activeMoveToPoint;
            StartCoroutine(CdTimerNewPoint(moveDelay));
        }
    }
    public IEnumerator CdTimerNewPoint(float cdTimer)
    {
        if (moveToIndex >= moveToPoints.Count)
        {
            moveToIndex = 0;
        }
        yield return new WaitForSeconds(cdTimer);
        TriggerMoveTo();
    }
    //
}
