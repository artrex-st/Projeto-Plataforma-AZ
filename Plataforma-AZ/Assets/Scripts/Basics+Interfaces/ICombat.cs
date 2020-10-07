using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICombat
{
    void ApplyDmg(float dmg);
    void ApplyDmg(float dmg, string type);
}
