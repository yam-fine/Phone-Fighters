using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[CreateAssetMenu(menuName = "Abilities/Shoot")]
public class Shoot : Ability
{

    [SerializeField]
    GameObject projectile;
    [SerializeField]
    int speed = 10;

    public override void Init()
    {
        return;
    }

    public override void TriggerAbility(Transform source)
    {
        GameObject shot = Instantiate(projectile,
                                      source.Find(string.Format("{0}_Pos", name)).transform.position,
                                      Quaternion.identity);
        shot.GetComponent<SwordCollider>().Init(damage, source);

        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
        {
            shot.GetComponent<Projectile>().Initialize(
                new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
                            CrossPlatformInputManager.GetAxis("Vertical")),
                speed);
        }
        else
        {
            if (source.rotation.y == 0)
                shot.GetComponent<Projectile>().Initialize(Vector2.right, speed);
            else
                shot.GetComponent<Projectile>().Initialize(Vector2.left, speed);
        }
    }
}
