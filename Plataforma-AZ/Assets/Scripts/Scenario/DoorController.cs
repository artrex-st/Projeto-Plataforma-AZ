using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorController : MonoBehaviour
{
    private Boolean isLocked = true;
    public int dorKeyCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerSM>().playerKeys >= dorKeyCost)
            {
                collision.gameObject.GetComponent<PlayerSM>().UseKeys(dorKeyCost);
                isLocked = false;
                Destroy(gameObject,1f);
            }
            
        }
    }
}
