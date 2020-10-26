using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    [Header("Enemy")]
    public GameObject active;
    [Header("Scan")]
    public float scanRange;
    public LayerMask targetLayer;
    public string tagTarget;
    [Header("Move To")]
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
        //TriggerCircleScan();
        TriggerMoveTo();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.ExecuteActiveState();
    }

    #region StatesTriggers
    public void TriggerMoveTo()
    {
        stateMachine.ChangeState(new MoveToTargetState(active,moveToSpeed,moveToMinRange,moveToMaxRange,moveToPoints,moveToIndex, MoveToDone));
    }
    private void TriggerCircleScan()
    {
        stateMachine.ChangeState(new ScanCircleState(active, scanRange, targetLayer, tagTarget, ScanFound));
    }
    #endregion
    public void ScanFound(ScanCircleResults scanResults)
    {
        var scanItens = scanResults.allCollScanTag;
        if (scanItens[0].transform.CompareTag("Player"))
        {
            Debug.Log("é player");
            moveToTarget = scanItens[0].transform;
            TriggerMoveTo();
        }
    }
    public void MoveToDone(MoveToResults moveToResult)
    {
        if (moveToIndex != moveToResult.activeMoveToPoint && moveToResult.patrolDone)
        {
            moveToIndex = moveToResult.activeMoveToPoint;
            StartCoroutine(CdTimerNewPoint(cdPatrolTimer));
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
    #region Function
    public IEnumerator CdTimerStartScan(float cdTimer)
    {
        Debug.Log("Esperando para voltar a scanear");
        yield return new WaitForSeconds(cdTimer);
        TriggerCircleScan();
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(active.transform.position, scanRange);
    }
}
