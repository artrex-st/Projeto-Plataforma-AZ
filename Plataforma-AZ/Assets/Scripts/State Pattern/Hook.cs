using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    public GameObject active;
    public float scanRange;
    public LayerMask targetLayer;
    public string tagTarget;
    [Header("Patrol")]
    public Transform moveToTarget;
    public float moveToSpeed;
    public float moveToMinRange;
    public float moveToMaxRange;
    public List<Transform> moveToPoints;
    public int moveToIndex;
    public float cdPatrolTimer;

    // Start is called before the first frame update
    void Start()
    {
        TriggerCircleScan();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.ExecuteActiveState();
    }

    #region StatesTriggers
    public void TriggerMoveTo()
    {
        stateMachine.ChangeState(new MoveToTargetState(active,moveToSpeed,moveToMinRange,moveToMaxRange,moveToPoints,moveToIndex,MoveToDone));
    }
    private void TriggerCircleScan()
    {
        stateMachine.ChangeState(new ScanCircleState(active, scanRange, targetLayer, tagTarget, ScanFound));
    }
    #endregion
    public void ScanFound(ScanCircleResults scanResults)
    {
        var scanItens = scanResults.allCollScanTag;
        if (scanItens.Count >=1)
        {
            moveToTarget = scanItens[0].transform;
            TriggerMoveTo();
        }
    }
    public void MoveToDone(MoveToResults moveToResult)
    {
        Debug.Log($"Patrol: {moveToIndex} e {moveToResult.activeMoveToPoint}");
        if (moveToIndex != moveToResult.activeMoveToPoint && moveToResult.patrolDone)
        {
            moveToIndex = moveToResult.activeMoveToPoint;
            StartCoroutine(CdTimerStartScan(cdPatrolTimer));
        }
    }
    public IEnumerator CdTimerStartScan(float cdTimer)
    {
        Debug.Log("Esperando para voltar a scanear");
        yield return new WaitForSeconds(cdTimer);
        TriggerCircleScan();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(active.transform.position, scanRange);
    }
}
