using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();
    [SerializeField]
    private GameObject active;
    [SerializeField]
    private float scanRange;
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private string tagTarget;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new ScanCircleState(active,scanRange,targetLayer,tagTarget,ScanFound));
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.ExecuteActiveState();
    }

    public void ScanFound(ScanCircleResults scanResults)
    {
        var scanItens = scanResults.allCollScanTag;
        if (scanItens.Count >= 1)
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public void TriggerPatrol()
    {
        stateMachine.ChangeState(new PatrolState());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(active.transform.position, scanRange);
    }
}
