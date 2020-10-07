using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnemy : MonoBehaviour, ICombat
{
    #region Variables
    public bool testeState;
    public enum States { Idle, Patrol, Attack1, Attack2, HookStart, HookEnd, HookBack, Hurt };
    public States state = States.Idle;
    public States preState;
    public string stateName;
    public bool Enemy;
    [Space(10), Header("Status")]
    public float maxHp;
    public float currHp;
    [Header("Damage of skills")]
    public float attack1Dmg;
    public float attack2Dmg;
    public float hookDmg;
    [Space(10)]
    [Header("Timers")]
    public float hookTime = 2;
    public float hookTimeBack = 1;
    public float attack1Time = 1;
    public float attack2Time = 2;
    [Space(10), Header("Ranges")]
    public float hookRange;

    [Space(5)]
    public bool changeState = false;
    [Header("Componentes")]
    public PlayerController player;
    public GameObject firePoint;
    public GameObject hookPrefab;
    public GameObject hookPrefabActive;
    public LineRenderer rayRender;
    public SpringJoint2D hookPull;

    #endregion
    public void ApplyDmg(float dmg)
    {
        currHp -= dmg;
    }

    public void ApplyDmg(float dmg, string type)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        currHp = maxHp;
        SwithCaseStateName();
    }

    // Update is called once per frame
    void Update()
    {
        if (testeState)
        {
            SetState(States.HookStart);
            changeState = true;
            testeState = false;
        }
        //TesteSwithCase();
        Invoke(stateName, 0);
        //PlayState();
    }

    // swith state name to run / troca o nome do stato para executar
    private void SwithCaseStateName()
    {
        switch (state)
        {
            case States.Idle:
                stateName = "IdleState";
                break;
            case States.Patrol:
                stateName = "Patrol";
                break;
            case States.Attack1:
                stateName =  "Attack1";
                break;
            case States.Attack2:
                stateName = "Attack2";
                break;
            case States.HookStart:
                stateName = "HookStart";
                break;
            case States.HookEnd:
                stateName = "HookEnd";
                break;
        }
    }
    // fim

    private void SetState(States newState)
    {
        if (changeState)
        {
            changeState = false;
            if (newState != state)
            {
                preState = state;
            }
            state = newState;
            SwithCaseStateName();
        }
    }
    #region States Implementations

    private void IdleState()
    {
        Debug.Log("Idle");
    }

    private void Patrol()
    {
        Debug.Log("Patrol");

    }

    private void Attack1()
    {
        Debug.Log("Ataack 1");
        StartCoroutine(Attack1Time());
    }
    public IEnumerator Attack1Time()
    {
        yield return new WaitForSeconds(attack1Time);
        changeState = true;
        Debug.LogWarning($"Hook Change: {States.Attack1} to {States.Idle}");
        SetState(States.Idle);
    }

    private void Attack2()
    {
        Debug.Log("Attack 2");
        StartCoroutine(Attack2Time());
    }
    public IEnumerator Attack2Time()
    {
        yield return new WaitForSeconds(attack2Time);
        changeState = true;
        Debug.LogWarning($"Hook Change: {States.Attack2} to {States.Idle}");
        SetState(States.Idle);
    }

    private void HookStart()
    {
        Debug.Log("Hook Start");
        if (state == States.HookStart && hookPrefabActive == null)
        {
            hookPrefabActive = Instantiate(hookPrefab, firePoint.transform.position, transform.rotation);
            hookPull.connectedBody = hookPrefabActive.GetComponent<Rigidbody2D>();
        }
        StartCoroutine(HookInComing());

    }

    public IEnumerator HookInComing()
    {
        yield return new WaitForSeconds(hookTime);
        changeState = true;
        if (state == States.HookStart)
        {
            hookPrefabActive.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 50, ForceMode2D.Impulse);
        }
        Debug.LogWarning($"Hook Change: {States.HookStart} to {States.HookEnd}");
        SetState(States.HookEnd);
    }
    private void HookEnd()
    {
        StartCoroutine(HookComingBack());
    }
    public IEnumerator HookComingBack()
    {
        Debug.Log("Hook End");
        SetLindeRender();


        hookPull.enabled = true;
        yield return new WaitForSeconds(hookTimeBack);
        changeState = true;
        Debug.LogWarning($"Hook Change: {States.HookEnd} to {States.Attack1}");
        SetState(States.Attack1);
    }

    #endregion
    private void SetLindeRender()
    {
        rayRender.enabled = true;
        rayRender.SetPosition(0, transform.position);
        rayRender.SetPosition(1, hookPrefabActive.transform.position);
        StartCoroutine(RayRenderOff());
    }
    public IEnumerator RayRenderOff()
    {
        yield return new WaitForSeconds(hookTimeBack);
        Destroy(hookPrefabActive.gameObject);
        rayRender.enabled = false;
        hookPull.enabled = false;

    }
    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, Vector2.right);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position - new Vector3(0.4f, 0.28f), new Vector3(0.36f, 2.2f, 1));
    }
    /*
     public bool OnDrainStart()
    {
        if (aiSight.sightState == AISight.SightStates.seeingEnemy)
        {
            return false;
        }

        StopAllCoroutines();
        searchLight.enabled = false;
        stunParticles.Play();
        aiEnabled = false;
        StartCoroutine(DrainRoutine());
        return true;
    }

    public IEnumerator DrainRoutine()
    {
        while (character.energyLeft>0)
        {
            character.energyLeft -=Time.deltaTime * player.drainSpeed;
            player.AddEnergy(Time.deltaTime * player.drainSpeed);
            SetEnergyFraction(character.energyLeft / character.maxDrainEnergy);
            yield return null;
        }
        Debug.Log("Fully drained");
        stunParticles.Stop();
        player.DrainOver();

    }
    //
     
     */

}