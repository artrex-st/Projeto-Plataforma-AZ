using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorController : MonoBehaviour
{
    private Boolean isLocked = true;
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
            if (collision.gameObject.GetComponent<PlayerController>().keys >= 1)
            {
                collision.gameObject.GetComponent<PlayerController>().UseKey();
                isLocked = false;
                Destroy(gameObject,1f);
            }
            
        }
    }
}
