using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[CreateAssetMenu(menuName = "Abilities/Hook")]
public class HookAbility : Ability {

    [SerializeField]
    GameObject prefab;

    public override void Init()
    {
        return;
    }

    public override void TriggerAbility(Transform source)
    {
        GameObject shot = Instantiate(prefab,
                              source.Find(string.Format("{0}_Pos", name)).transform.position,
                              Quaternion.identity);
        shot.GetComponent<SwordCollider>().Init(damage, source);

        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
        {
            shot.GetComponent<Hook>().Initialize(
                new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
                            CrossPlatformInputManager.GetAxis("Vertical")));
        }
        else
        {
            if (source.rotation.y == 0)
                shot.GetComponent<Hook>().Initialize(Vector2.right);
            else
                shot.GetComponent<Hook>().Initialize(Vector2.left);
        }
    }
}
