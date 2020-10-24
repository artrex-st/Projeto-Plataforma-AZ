using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Boolean isLocked = true;
    public int playerKeyCost;
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
            if (collision.gameObject.GetComponent<PlayerSM>().playerKeys >= playerKeyCost)
            {
                collision.gameObject.GetComponent<PlayerSM>().UseKeys(playerKeyCost);
                isLocked = false;
            }

        }
    }
}
