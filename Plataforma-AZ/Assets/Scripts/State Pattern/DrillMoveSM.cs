using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMoveSM : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    public GameObject active;
    public bool activeMove;
    [Header("Patrol")]
    public MoveMode moveMode = MoveMode.Vertical;
    public Transform moveToTarget;
    public float moveToSpeed;
    public float moveToMinRange;
    public float moveToMaxRange;
    public List<Transform> moveToPoints;
    public int moveToIndex;
    public float moveDelay;
    public float drillDMG;
    


    void Start()
    {
        if (activeMove)
        {
            TriggerMoveTo();
        }
    }

    void Update()
    {
        if (activeMove)
        {
            stateMachine.ExecuteActiveState();
        }
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

    //andar junto com a plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ICombat>().ApplyDmg(drillDMG);
            Debug.Log("Damage ON");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //collision.transform.SetParent(null);
        }
    }
}
