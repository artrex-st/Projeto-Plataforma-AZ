using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArtrexUtils
{
    public static class Check
    {
        /// <summary>
        /// Classe para vericar toque de Layers, precisa do Ponto de toque e qual layer ele deve verificar. (transform do pé + Layer do chão)
        /// </summary>
        /// <param name="footPosition">Posição para o verificar o toque</param>
        /// <param name="layerOfGround">Qual deve ser a layer verificada</param>
        /// <returns></returns>
        public static Boolean FGrounded(Transform footPosition, LayerMask layerOfGround)
        {
            return Physics2D.OverlapCircle(footPosition.transform.position, 0.2f, layerOfGround);
        }
        /// <summary>
        /// Classe criada para Verificar se o numero é Maior ou igual a 0 ou se é menor ou igual a Zero,
        /// </summary>
        /// <param name="velocity">Valor para comparação</param>
        /// <returns></returns>
        public static bool Flip2D(float velocity)
        {
            if (velocity <= 0)
                return true;//SpritePlayer.flipX = true;
            else
            if (velocity >= 0)
                return false; //SpritePlayer.flipX = false;
            else
                return false;
        }
    }
}
