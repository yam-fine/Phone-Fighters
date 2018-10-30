using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SwordCollider))]
public class Hook : MonoBehaviour {

    [SerializeField]
    float timeForward = 0, timeBack = 0, pauseTime = 0;
    List<Transform> enemies = new List<Transform>();
    public List<Transform> Enemies { get {return enemies; }}
    Vector3 localScale;
    Vector3 scaleTo;
    SwordCollider sc;
    Vector2 direction;

    void Start ()
    {
        localScale = transform.localScale;
        scaleTo = new Vector3(0, localScale.y, localScale.z);
        transform.localScale = scaleTo;
        Rotate();
        sc = GetComponent<SwordCollider>();
        StartCoroutine(GrowerNotShower());
    }

    IEnumerator GrowerNotShower()
    {
        float time = 0;
        while (time < timeForward * Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                localScale,
                (time /(timeForward * Time.deltaTime)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = localScale;
        time = 0;

        yield return new WaitForSeconds(pauseTime);        

        while (time < timeBack * Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                scaleTo,
                (time /(timeBack * Time.deltaTime)));
            Pull(transform.position, (time / (timeBack * Time.deltaTime)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = scaleTo;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == sc.TargetTag)
        {
            enemies.Add(other.transform);
        }
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void Pull(Vector3 destination, float time)
    {
        foreach (Transform enemy in enemies)
        {
            if (enemy != null)
                enemy.position = Vector3.Lerp(enemy.position, 
                                              transform.position, 
                                              time);
        }
    }

    void Rotate()
    {
        Debug.Log(direction);
        if (direction.x >= 0 && direction.x < 180)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
