using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloundSpawn : MonoBehaviour
{
    public Transform cloudStartSpawn;
    public Transform cloudEndSpawn;
    public float timeSpawnCD;
    public float rangeSpawn;
    public float cloudMinSpeed;
    public float cloudMaxSpeed;
    public int maxCloudCreated;
    public GameObject cloudPreFab;
    public Sprite[] cloudSprite;
    public List<GameObject> cloudInstance;
    public bool cloudCD;



    void Start()
    {
        cloudInstance.Add(Instantiate(cloudPreFab, cloudStartSpawn.position + new Vector3(0, UnityEngine.Random.Range(-rangeSpawn, rangeSpawn), 0), transform.rotation));
        //cloundCD = true;

    }

    void Update()
    {
        if (!cloudCD && cloudInstance.Count < maxCloudCreated)
        {
            cloudCD = true;
            StartCoroutine(SpawnCloud());
        }
        else if (cloudInstance[0].transform.position.x >= cloudEndSpawn.position.x)
        {
            Destroy(cloudInstance[0].gameObject);
            cloudInstance.RemoveAt(0);
        }
        for (int i = 0; i < cloudInstance.Count; i++)
        {
            cloudInstance[i].transform.position = Vector3.MoveTowards(cloudInstance[i].transform.position, new Vector3(cloudEndSpawn.position.x, cloudInstance[i].transform.position.y), UnityEngine.Random.Range(cloudMinSpeed, cloudMaxSpeed) * Time.deltaTime);
        }
    }

    private IEnumerator SpawnCloud()
    {
        yield return new WaitForSecondsRealtime(timeSpawnCD);
        cloudInstance.Add(Instantiate(cloudPreFab, cloudStartSpawn.position + new Vector3(0,UnityEngine.Random.Range(-rangeSpawn,rangeSpawn),0), transform.rotation));
        int index = cloudInstance.Count - 1;
        cloudInstance[index].GetComponent<SpriteRenderer>().sprite = cloudSprite[UnityEngine.Random.Range(0,cloudSprite.Length)];
        Debug.Log(cloudInstance.Count);
        cloudCD = false;

    }
}
