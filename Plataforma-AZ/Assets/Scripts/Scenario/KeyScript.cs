using UnityEngine;
[RequireComponent(typeof(Collider2D))]

public class KeyScript : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;
    private void Awake()
    {
        transform.tag = "Key";
    }
    private void Start()
    {
        KeyAnimation();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().keys++;
            Destroy(gameObject, 0.1f);
        }
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, curve.Evaluate(Time.time), transform.position.z);
    }
    public void KeyAnimation()
    {
        curve = new AnimationCurve(new Keyframe(0, transform.position.y), new Keyframe(0.5f, transform.position.y + 0.5f), new Keyframe(1, transform.position.y));
        curve.preWrapMode = WrapMode.Loop;
        curve.postWrapMode = WrapMode.Loop;
        curve.SmoothTangents(0,0f);
        curve.SmoothTangents(1,5f);
        curve.SmoothTangents(2,0f);

    }
}
