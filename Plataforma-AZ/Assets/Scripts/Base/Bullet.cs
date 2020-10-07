using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public enum BulletType { AP, Explosion, Hook, Poison };
    public BulletType bulletType;
    public GameObject player;
    public SpriteRenderer spriteBase;
    public Sprite[] spriteNew;
    public bool enemy;
    public float timeDestruction;
    void Start()
    {
        StartCoroutine(SelfDestruction());
 
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.gameObject.GetComponent<PlayerController>().stun == true)
        {
            //player.transform.position = transform.position;
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(transform.position.x, transform.position.y, player.transform.position.z), 5 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), 5 * Time.deltaTime);

        }
    }

    private void TesteSwithCase()
    {
        switch (bulletType)
        {
            case BulletType.AP:
                AP();
                break;
            case BulletType.Explosion:
                Explosion();
                break;
            case BulletType.Hook:
                Hook();
                break;
            case BulletType.Poison:
                Poison();
                break;
        }
    }

    IEnumerator SelfDestruction()
    {
        yield return new WaitForSecondsRealtime(timeDestruction);
        Destroy(gameObject);
    }

    public void AP()
    {

    }
    public void Explosion()
    {

    }
    public void Hook()
    {

    }
    public void Poison()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Colediu");
        if (enemy && collision.gameObject.CompareTag("Player"))
        {
            if (BulletType.Hook == bulletType)
            {
                spriteBase.sprite = spriteNew[1];
                collision.gameObject.GetComponent<PlayerController>().stun = true;
                player = collision.gameObject;
                
            }

        }
    }
}

