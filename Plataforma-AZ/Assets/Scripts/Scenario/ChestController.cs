using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Boolean isLocked = true;
    [SerializeField]
    private GameObject coverOpen;

    void Update()
    {
        if (!isLocked)
        {
            coverOpen.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isLocked)
        {
            if (collision.gameObject.GetComponent<PlayerController>().keys >= 1)
            {
                collision.gameObject.GetComponent<PlayerController>().UseKey();
                isLocked = false;
            }

        }
    }
}
