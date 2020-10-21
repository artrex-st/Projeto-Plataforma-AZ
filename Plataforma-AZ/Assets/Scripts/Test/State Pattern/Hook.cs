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
    public Transform patrolTarget;
    public float patrolSpeed;
    public float patrolMinRange;
    public float patrolMaxRange;
    public List<Transform> patrolPoints;
    public int patrolIndex;

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
    public void TriggerPatrol()
    {
        stateMachine.ChangeState(new PatrolState(active,patrolTarget,patrolSpeed,patrolMinRange,patrolMaxRange,patrolPoints,patrolIndex,PatrolDone));
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
            patrolTarget = scanItens[0].transform;
            TriggerPatrol();
        }
    }
    public void PatrolDone(PatrolResults patrolResult)
    {
        Debug.Log($"Patrol: {patrolIndex} e {patrolResult.activePatrolPoint}");
        if (patrolIndex != patrolResult.activePatrolPoint && patrolResult.patrolDone)
        {
            patrolIndex = patrolResult.activePatrolPoint;
            StartCoroutine(CdTimerStartScan(2));
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
