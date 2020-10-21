using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    protected Character character;
    public virtual void Start() { }
    public virtual void OnStay() { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnTakeDmg() { }
    public virtual void OnDealDmg() { }

    public State(Character character)
    {
        this.character = character;
    }
}
