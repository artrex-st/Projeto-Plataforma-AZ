using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Boolean isPunching;
    [SerializeField]
    private float cdAttack, attackTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && PlayerController.isGround && cdAttack >= attackTime)
        {
            StartCoroutine(Punch());
            cdAttack = 0;
        }
        if (Input.GetButton("Fire3") && PlayerController.isGround && cdAttack >= attackTime)
        {
            StartCoroutine(Kick());
            cdAttack = 0;
        }
        cdAttack += Time.deltaTime;
    }
    IEnumerator Punch()
    {
        Debug.Log("Punch");
        PlayerController.isPunching = true;
        yield return 0.02f;
        Debug.Log("fim Punch");
    }
    IEnumerator Kick()
    {
        Debug.Log("Kick");
        PlayerController.isKicking = true;
        yield return 0.02f;
        Debug.Log("End Kick");
    }
}
