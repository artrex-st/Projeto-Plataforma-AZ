using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Boolean isPunching;
    [SerializeField]
    private float cdAttack, attackTime, range, dmg;
    public GameObject[] attackPoint;
    [SerializeField]
    private GameObject attackPointActive;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        attackPoint = GameObject.FindGameObjectsWithTag("AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && PlayerController.isGround && cdAttack >= attackTime && !PlayerController.isGroundSlide && !PlayerController.isPunching)
        {
            StartCoroutine(Punch());
            cdAttack = 0;
        }
        if (Input.GetButton("Fire3") && PlayerController.isGround && cdAttack >= attackTime && !PlayerController.isGroundSlide)
        {
            StartCoroutine(Kick());
            cdAttack = 0;
        }
        cdAttack += Time.deltaTime;
    }
    IEnumerator Punch()
    {
        PlayerController.isPunching = true;
        yield return 0.2f;
        AttackPointFlip();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPointActive.transform.position, new Vector2(range, 2 * range), 0);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.LogWarning("Melee Enemy Hit in:" + enemy.name);
                enemy.GetComponent<Rigidbody2D>().AddForce(GetComponentInChildren<SpriteRenderer>().flipX ? new Vector2(-dmg, dmg)  : new Vector2(dmg, dmg), ForceMode2D.Impulse);
            }
            Debug.Log("Melee Hit in:" + enemy.name);
        }
    }
    IEnumerator Kick()
    {
        PlayerController.isKicking = true;
        yield return 0.02f;
        AttackPointFlip();

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPointActive.transform.position, new Vector3(range, 2 * range, 1));

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position - new Vector3(0.4f, 0.28f), new Vector3(0.36f, 2.2f, 1));
    }

    private void AttackPointFlip()
    {
        if (GetComponentInChildren<SpriteRenderer>().flipX)
        {
            attackPointActive = attackPoint[1];
        }else
            attackPointActive = attackPoint[0];
    }
}
