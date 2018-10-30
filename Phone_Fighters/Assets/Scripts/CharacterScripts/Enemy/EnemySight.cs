using UnityEngine;

public class EnemySight : MonoBehaviour {

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemy.OnDeath += (() => GetComponent<BoxCollider2D>().enabled = false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.InRange = false;
        }
    }
}
