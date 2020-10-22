using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCircleState : IStates
{
    private GameObject active;
    private float scanRange;
    private LayerMask targetLayer;
    private string tagTarget;
    public bool scanDone;
    private System.Action<ScanCircleResults> scanResultsCallBack;

    #region Contructor
    /// <summary>
    /// Circle base informations.
    /// </summary>
    /// <param name="active">Game object base Position</param>
    /// <param name="scanRange">Distance of Circle</param>
    /// <param name="targetLayer">Target Layer Filter</param>
    /// <param name="tagTarget">Tag of Target</param>
    public ScanCircleState(GameObject active, float scanRange, LayerMask targetLayer, string tagTarget, System.Action<ScanCircleResults> scanResultsCallBack)
    {
        this.active = active;
        this.scanRange = scanRange;
        this.targetLayer = targetLayer;
        this.tagTarget = tagTarget;
        this.scanResultsCallBack = scanResultsCallBack;
    }
    #endregion
    public void EnterState()
    {
        Debug.Log("Entrou no Scan Circle");
    }

    public void ExecuteState()
    {
        if (!scanDone)
        {
            var hitResults = Physics2D.OverlapCircleAll(active.transform.position, scanRange, targetLayer);
            // Package para entregar
            var allObjInTag = new List<Collider2D>();
            //
            for (int i = 0; i < hitResults.Length; i++)
            {
                if (hitResults[i].transform.CompareTag(tagTarget))
                {
                    Debug.Log($"overlap Circle acertou: {hitResults[i].transform.name}");
                    allObjInTag.Add(hitResults[i]);
                }
            }
            // Creating the Package
            var scanCircleResults = new ScanCircleResults(hitResults, allObjInTag);
            scanResultsCallBack(scanCircleResults);
            //
            //scanDone = true;
        }
    }
    public void ExitState()
    {
        Debug.Log("SAIU do Scan Circle");
    }
}
public class ScanCircleResults
{
    public Collider2D[] allCollInScan;
    public List<Collider2D> allCollScanTag;

    public ScanCircleResults(Collider2D[] allCollInScan, List<Collider2D> allCollScanTag)
    {
        this.allCollInScan = allCollInScan;
        this.allCollScanTag = allCollScanTag;
    }
}
