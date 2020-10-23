using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ArtrexUtils
{
    public static class Check
    {
        /// <summary>
        /// Classe para vericar "toque" de Layers. (Class created to check "touches".)
        /// </summary>
        /// <param name="footPosition">Local para o verificar o "toque" (position of "touch" check)</param>
        /// <param name="layerOfGround">layer para checar (layor to check)</param>
        /// <returns></returns>
        public static Boolean FGrounded(Transform footPosition, LayerMask layerOfGround)
        {
            return Physics2D.OverlapCircle(footPosition.transform.position, 0.2f, layerOfGround);
        }



        /// <summary>
        /// Classe criada para Verificar se o player está indo para direita ou esquerda e Virar a sprite da maneira correta. (Class created to check the player's direction, if player runs right, left or stop, he rotates the correct sprite)
        /// </summary>
        /// <param name="velocity">Valor para checar (value to check)</param>
        /// <param name="side">Valor para quando estiver parado (value for when stopped)</param>
        /// <returns></returns>
        public static quaternion Flip2D(float velocity, Quaternion side)
        {
            if (velocity >= 0)
                return new Quaternion(0, 0, 0, 0); //correndo para direita;
            else
            if (velocity < 0)
                return new Quaternion(0, 180, 0, 0); //correndo para a esquerda;
            else
                return side; // está parado
        }
    }
}
