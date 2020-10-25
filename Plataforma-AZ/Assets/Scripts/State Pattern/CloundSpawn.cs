using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloundSpawn : MonoBehaviour
{
    private List<GameObject> clound;
    public float selfDestructionTime;
    public GameObject cloundPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        GameObject cloundInstance = Instantiate(cloundPrefab, transform.position, transform.rotation);
        
    }
}
