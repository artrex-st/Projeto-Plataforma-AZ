using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy : MonoBehaviour
{
    public enum States { Idle, Patrol, Combat, Lost };
    public States state = States.Idle;
    public States preState;
    [Header("TESTES")]
    public bool teste;

    [Header("Status")]
    public float maxHealt;
    public float currHealt;
    public float dmg;
    public float speed;
    public int activePatrolPoint;
    public Transform activeTarget;
    public Transform preTarget;

    [Header("Timers")]
    public float timerIdle;
    public float timerPatrol;
    public float timerToLost;
    public float timer;
    public float timerMeleeHit;

    [Header("Ranges")]
    public float maxRangePatrol;
    public float maxRangeDetect;
    public float minRangeCombat;
    public float maxRangeCombat;
    public float maxMeleeHit;
    public float rangeMeleeAttack;

    [Header("Checking")]
    public bool runingState;
    public bool onMeleeAttack;

    [Header("Componentes")]
    public GameObject playerObj;
    public Rigidbody2D enemyBody;
    public SpriteRenderer enemySprite;
    public Transform[] patrolPoints;
    //fire points
    public Transform firePointActive;
    // vectors
    public Vector2 activeVector;
    public Vector2 scanCombatArea;
    // animation
    public LayerMask layerTarget;

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        ResetStates();
    }
    void Update()
    {
        switch (state)
        {
            case States.Idle:
                Idle();
                break;
            case States.Patrol:
                Patrol();
                break;
            case States.Combat:
                Combat();
                break;
            case States.Lost:
                break;
            default:
                break;
        }
    }
    private void Idle()
    {
        SetState(States.Patrol);
    }
    private void Patrol()
    {
        ScanPatrolArea();
        if (Mathf.Abs(transform.position.x - activeTarget.position.x) > maxRangePatrol && Mathf.Abs(transform.position.y - activeTarget.position.y) <= 1) //o 1 é para patrulha em cima
        {
            Move(activeTarget);
        }
        else
        {
            timer += Time.deltaTime;
            if (timerPatrol <= timer)
            {
                PatrolUpdate();
                timer = 0;
            }
        }
    }
    private void Combat()
    {
        ScanCombatArea();
        ChekFlip();
        if (Mathf.Abs(transform.position.x - activeTarget.position.x) > minRangeCombat && Mathf.Abs(transform.position.x - activeTarget.position.x) < maxRangeCombat)
        {
            Move(activeTarget);
        }
        else if (Mathf.Abs(transform.position.x - activeTarget.position.x) <= maxMeleeHit && !onMeleeAttack)
        {
            StartCoroutine(MeleeHitAttack());
            onMeleeAttack = true;
        }

    }
    public void MeleeAttack()
    {

    }
    IEnumerator MeleeHitAttack()
    {
        yield return new WaitForSeconds(timerMeleeHit);
        onMeleeAttack = false;
        Collider2D hitEnemies = Physics2D.OverlapCircle(firePointActive.transform.position, rangeMeleeAttack, layerTarget);
        if (hitEnemies.CompareTag("Player"))
        {
            hitEnemies.GetComponent<ICombat>().ApplyDmg(dmg);
            hitEnemies.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(dmg * activeVector.x ,dmg), firePointActive.position,ForceMode2D.Impulse);
            Debug.Log("Acertou melee hit");
        }

    }
    private void Move(Transform target)
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
    }
    private void ChekFlip()
    {
        enemySprite.flipX = transform.position.x <= activeTarget.position.x ? false : true;
        if (enemySprite.flipX)
        {
            firePointActive.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
            activeVector = Vector2.left;
        }
        else
        {
            firePointActive.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            activeVector = Vector2.right;
        }
    }
    private void PatrolUpdate()
    {
        if (preState != States.Patrol && preState != States.Idle)
        {
            SetTarget(preTarget);
        }
        else if (activePatrolPoint >= patrolPoints.Length - 1)
        {
            activePatrolPoint = 0;
        }
        else
            activePatrolPoint++;
        SetTarget(patrolPoints[activePatrolPoint]);
    }

    private void SetTarget(Transform newTarget)
    {
        if (newTarget != activeTarget)
        {
            preTarget = activeTarget;
        }
        activeTarget = newTarget;
        ChekFlip();
    }
    private void SetState(States newState)
    {
        if (newState != state)
        {
            preState = state;
        }
        state = newState;
    }
    private void ResetStates()
    {
        StopAllCoroutines();
        state = States.Idle;
        SetTarget(patrolPoints[activePatrolPoint]);
    }
    public void ScanPatrolArea()
    {
        RaycastHit2D hitScan = Physics2D.Raycast(firePointActive.transform.position, activeVector, maxRangeDetect, layerTarget);
        if (hitScan.collider != null && hitScan.transform.CompareTag("Player"))
        {
            SetState(States.Combat);
            SetTarget(hitScan.transform);
        }
    }
    public void ScanCombatArea()
    {
        Collider2D hitCombatArea = Physics2D.OverlapBox(transform.position, scanCombatArea, 0, layerTarget);
        if (hitCombatArea != null)
        {
            Debug.Log("Ainda em combate PA-PAPA");
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timerToLost <= timer)
            {
                PatrolUpdate();
                SetState(States.Idle);
                timer = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePointActive.transform.position, new Vector2(firePointActive.transform.position.x + maxRangeDetect * activeVector.x, firePointActive.transform.position.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, scanCombatArea);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(firePointActive.position, rangeMeleeAttack);

    }
}
