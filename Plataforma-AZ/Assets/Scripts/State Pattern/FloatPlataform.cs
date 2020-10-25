using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlataform : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    public GameObject active;
    public enum MoveMode { Diagonal, Horizontal, Vertical}
    public MoveMode moveMode = MoveMode.Diagonal;
    [Header("Patrol")]
    public Transform moveToTarget;
    public float moveToSpeed;
    public float moveToMinRange;
    public float moveToMaxRange;
    public List<Transform> moveToPoints;
    public int moveToIndex;
    public float moveDelay;
    private int moveModeIndex;


    void Start()
    {
        ChoseMove();
        TriggerMoveTo();
    }

    void Update()
    {
        stateMachine.ExecuteActiveState();
    }

    #region trigger
    private void TriggerMoveTo()
    {
        stateMachine.ChangeState(new MoveToTargetState(active, moveToSpeed, moveToMinRange, moveToMaxRange, moveToPoints, moveToIndex, MoveToDone, moveModeIndex));
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

    private void ChoseMove()
    {
        switch (moveMode)
        {
            case MoveMode.Diagonal:
                moveModeIndex = 0;
                break;
            case MoveMode.Horizontal:
                moveModeIndex = 1;
                break;
            case MoveMode.Vertical:
                moveModeIndex = 2;
                break;
            default:
                break;
        }
    }
    //
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
