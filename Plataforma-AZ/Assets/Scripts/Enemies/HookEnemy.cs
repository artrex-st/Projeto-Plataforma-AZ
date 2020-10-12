using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnemy : MonoBehaviour, ICombat
{
    // teste
    public bool test;
    public bool testeState;
    //
    #region Variables
    public enum States { Idle, Patrol, Attack1, Attack2, HookStart, HookEnd, HookBack, Hurt, TargetLock, LostTarget};
    public States state = States.Idle;
    public States preState;
    public string stateName;

    [Space(10), Header("Status")]
    public float maxHp;
    public float currHp;
    public float speed;
    public int currentPatrolPoint;

    [Space(5), Header("Chek Variables")]
    public bool Enemy;
    public bool changeState = false;
    public bool targetLock = false;

    [Header("Damage of skills")]
    public float attack1Dmg;
    public float attack2Dmg;
    public float hookDmg;

    [Space(10), Header("Timers")]
    public float hookTime = 2;
    public float hookTimeBack = 1;
    public float attack1Time = 1;
    public float attack2Time = 2;
    public float resumePatrolTime = 2;

    [Space(10), Header("Ranges")]
    public float hookRange;
    public float maxEngageRange;
    public float maxDetectRange;
    public float minHookStart;
    public float maxMeleeRange;

    [Space(10), Header("Componentes")]
    public Vector2 vectorRight, VectorLeft, activeVector;
    public GameObject playerObj;
    public GameObject firePointLeft;
    public GameObject firePointRight;
    public GameObject firePointActive;
    public GameObject hookPrefab;
    public GameObject hookPrefabActive;
    public LineRenderer rayRender;
    public SpringJoint2D hookPull;
    public Transform currentTarget;
    public Transform preTarget;
    public Transform[] patrolPoint;
    public Rigidbody2D enemyBody;
    public LayerMask layerTarget;
    public SpriteRenderer enemySprite;



    #endregion

    void Start()
    {
        currHp = maxHp;
        SwithCaseStateName();
        enemyBody = GetComponent<Rigidbody2D>();
        currentTarget = patrolPoint[currentPatrolPoint];
        playerObj = GameObject.FindGameObjectWithTag("Player");
        enemySprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testeState)
        {
            SetState(States.Patrol);
            changeState = true;
            testeState = false;
        }
        //TesteSwithCase();
        Invoke(stateName, 0);
        //test = patrolPoint[0].position;
        //PlayState();
    }
    public void ApplyDmg(float dmg)
    {
        currHp -= dmg;
    }

    public void ApplyDmg(float dmg, string type)
    {
        throw new System.NotImplementedException();
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
            case States.Hurt: // criar as funçoes
                stateName = "Hurt";
                break;
        }
    }
    // fim
    private void Move(Transform target)
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);   
    }
    private void TargetUpdate()
    {
        if (state.Equals(States.Patrol))
        {
            currentTarget = patrolPoint[currentPatrolPoint];
            if (currentPatrolPoint >= patrolPoint.Length - 1)
            {
                currentPatrolPoint = 0;
            }
            else
                currentPatrolPoint++;
        }
        else if (state.Equals(States.Idle))
            currentTarget = patrolPoint[currentPatrolPoint];
        ChekFlip();
    }
    private void ChekFlip()
    {
        enemySprite.flipX = transform.position.x <= currentTarget.position.x ? false : true;
        if (enemySprite.flipX)
        {
            firePointActive = firePointLeft;
            activeVector = Vector2.left;
        }
        else
        {
            firePointActive = firePointRight;
            activeVector = Vector2.right;
        }
    }
    public void ScanArea()
    {
        RaycastHit2D[] hitScan = Physics2D.RaycastAll(firePointActive.transform.position, activeVector, maxDetectRange, layerTarget);
        foreach (RaycastHit2D target in hitScan)
        {
            if (target.transform.CompareTag("Player"))
            {
                StopAllCoroutines(); // para as rotinas de patrulha e idle
                SetTarget(target.transform);
                changeState = true;
                targetLock = true;
                if (Vector2.Distance(transform.position, currentTarget.position) > minHookStart)
                {
                    SetState(States.HookStart);
                }else if (Vector2.Distance(transform.position, currentTarget.position) < maxMeleeRange)
                {
                    SetState(States.Attack1);
                }

                //enemy.GetComponent<Rigidbody2D>().AddForce(GetComponentInChildren<SpriteRenderer>().flipX ? new Vector2(-dmg, dmg) : new Vector2(dmg, dmg) * 2, ForceMode2D.Impulse);
            }
            else if (!state.Equals(States.Idle))
            {
                changeState = true;
                SetState(States.Idle);
            }

            Debug.Log("Scan found:" + target.transform.name);
        }
    }

    #region Seters
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
    private void SetTarget(Transform newTarget)
    {
        if (changeState)
        {
            changeState = false;
            if (newTarget != currentTarget)
            {
                preTarget = currentTarget;
            }
            currentTarget = newTarget;
            TargetUpdate();
        }
    }
    private void SetLindeRender()
    {
        rayRender.enabled = true;
        rayRender.SetPosition(0, transform.position);
        rayRender.SetPosition(1, hookPrefabActive.transform.position);
        StartCoroutine(RayRenderOff());
    }
    #endregion // REGION SET
    #region States Implementations

    private void IdleState()
    {
        Debug.Log("Idle");
        TargetUpdate();
        StartCoroutine(StartPatrol());
    }
    private void Patrol()
    {

        ScanArea();
        if (Vector2.Distance(transform.position, currentTarget.position) > maxEngageRange)
        {
            Move(currentTarget); // a mudança de target ainda nao funciona o target
        }else
        {
            changeState = true;
            SetState(States.Idle);
        }
    }
    private void Attack1()
    {
        Debug.Log("Ataack 1");
        StartCoroutine(Attack1Time());
    }
    private void Attack2()
    {
        Debug.Log("Attack 2");
        StartCoroutine(Attack2Time());
    }
    private void HookStart()
    {
        Debug.Log("Hook Start");
        if (state == States.HookStart && hookPrefabActive == null)
        {
            hookPrefabActive = Instantiate(hookPrefab, firePointActive.transform.position, transform.rotation);
            hookPull.connectedBody = hookPrefabActive.GetComponent<Rigidbody2D>();
            hookPrefabActive.GetComponent<SpriteRenderer>().flipX = !enemySprite.flipX;
        }
        StartCoroutine(HookInComing());

    }
    private void HookEnd()
    {
        StartCoroutine(HookComingBack());
    }
    public void Hurt()
    {

    }
    #endregion // REGION
    #region Enumerators
    public IEnumerator StartPatrol()
    {
        yield return new WaitForSecondsRealtime(resumePatrolTime);
        changeState = true;
        SetState(States.Patrol);
    }
    public IEnumerator Attack1Time()
    {
        yield return new WaitForSeconds(attack1Time);
        changeState = true;
        Debug.LogWarning($"Hook Change: {States.Attack1} to {States.Idle}");
        SetState(States.Idle);
    }
    public IEnumerator Attack2Time()
    {
        yield return new WaitForSeconds(attack2Time);
        changeState = true;
        Debug.LogWarning($"Hook Change: {States.Attack2} to {States.Idle}");
        SetState(States.Idle);
    }
    public IEnumerator HookInComing()
    {
        yield return new WaitForSeconds(hookTime);
        changeState = true;
        if (state == States.HookStart)
        {
            hookPrefabActive.GetComponent<Rigidbody2D>().AddForce(activeVector * 50, ForceMode2D.Impulse);
        }
        Debug.LogWarning($"Hook Change: {States.HookStart} to {States.HookEnd}");
        SetState(States.HookEnd);
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
    public IEnumerator RayRenderOff()
    {
        yield return new WaitForSeconds(hookTimeBack);
        Destroy(hookPrefabActive.gameObject);
        rayRender.enabled = false;
        hookPull.enabled = false;

    }
    #endregion // REGION


    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, Vector2.right);

        Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(firePoint.transform.position, new Vector3(maxDetectRange * 3, maxDetectRange, 0));
        Gizmos.DrawLine(firePointActive.transform.position, new Vector2(firePointActive.transform.position.x + maxDetectRange * activeVector.x, firePointActive.transform.position.y));

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