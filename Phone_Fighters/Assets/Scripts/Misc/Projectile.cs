using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    [SerializeField]
    float speed;
    Rigidbody2D myRigidbody;
    Vector2 direction;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        myRigidbody.AddRelativeForce(direction * Time.deltaTime * speed, ForceMode2D.Impulse);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }
}