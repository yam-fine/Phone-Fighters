using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MoveToNearest")]
public class MoveToNearest : Ability {

    [SerializeField]
    int speed = 1500;
    [SerializeField]
    string colliderName;
    Collider2D collider;
    float closestDistanceSqr = Mathf.Infinity;
    Transform nearestTarget = null;
    Player player;
    Vector2 direction;

    public override void Init()
    {
        closestDistanceSqr = Mathf.Infinity;
        nearestTarget = null;
        collider = NetworkManager.Instance.MyPlayer.transform.Find(colliderName).GetComponent<Collider2D>();
    }

    public override void TriggerAbility(Transform source)
    {
        Transform target = NearestTarget(source);
        if (!player)
            player = source.GetComponent<Player>();

        if (target)
        {
            if (source.rotation.y == 0)
            { direction = (new Vector3(target.position.x - 5, target.position.y)) - source.position; }
            else
            { direction = (new Vector3(target.position.x + 5, target.position.y)) - source.position; }

            player.ChangeRotation(target.position);
            Melee(source);
            player.StartCoroutine(player.CloseDistance(direction, speed, (Mathf.Sqrt(direction.sqrMagnitude) / (speed * Time.deltaTime))));
        }
    }

    Transform NearestTarget(Transform from)
    {
        closestDistanceSqr = Mathf.Infinity;
        nearestTarget = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector2 directionToTarget = enemy.transform.position - from.position; // enemy pos - my pos
            float dSqrToTarget = directionToTarget.sqrMagnitude; // using square distance as its more efficient than vector3.dist
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestTarget = enemy.transform;
            }
        }

        return nearestTarget;
    }

    void Melee(Transform source)
    {
        collider.GetComponent<SwordCollider>().Init(damage, source);

        collider.enabled = true;
        Vector2 tmpPos = collider.transform.position;
        collider.transform.position = new Vector3(tmpPos.x, tmpPos.y, 1);
        collider.transform.position = tmpPos;
    }
}
