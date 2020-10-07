using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    /// <summary>
    /// Classe de interface para atribuir direção de um Vector2.
    /// </summary>
    /// <param name="velocityVector">Vetores para volocidade X e Y</param>
    void SetVelocity(Vector2 velocityVector);
}
