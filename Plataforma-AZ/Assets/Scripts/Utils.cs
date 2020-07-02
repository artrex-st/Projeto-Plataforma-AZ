using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArtrexUtils
{
    public static class Check
    {
        public static Boolean FGrounded(Transform footPosition, LayerMask layerOfGround)
        {
            return Physics2D.OverlapCircle(footPosition.transform.position, 0.2f, layerOfGround);
        }
    }

}
