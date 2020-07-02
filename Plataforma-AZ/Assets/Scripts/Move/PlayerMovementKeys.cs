using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementKeys : MonoBehaviour
{

    private void Update()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        GetComponent<IMove>().SetVelocity(moveVector);

    }
}
