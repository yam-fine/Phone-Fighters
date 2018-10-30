using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class Melee : Ability
{

    [SerializeField]
    string colliderName;
    Collider2D collider;

    public override void Init()
    {
        return;
    }

    public override void TriggerAbility(Transform source)
    {
        collider = source.Find(colliderName).GetComponent<Collider2D>();

        collider.GetComponent<SwordCollider>().Init(damage, source);

        collider.enabled = true;
        Vector2 tmpPos = collider.transform.position;
        collider.transform.position = new Vector3(tmpPos.x, tmpPos.y, 1);
        collider.transform.position = tmpPos;
    }
}
