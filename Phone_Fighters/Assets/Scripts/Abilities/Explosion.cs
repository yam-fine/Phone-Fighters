using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Explosion")]
public class Explosion : Ability
{

    [SerializeField]
    GameObject explosion;
    [Tooltip("Camera Shake")]
    [SerializeField]
    float amplitude = 0, frequency = 0, time = 0;
    Transform pos;
    CameraScript camera;

    //try finding how to make this relevent to enemy
    public override void Init()
    {
        pos = NetworkManager.Instance.MyPlayer.transform.Find(string.Format("{0}_Pos", name)).transform;
        camera = CameraScript.Instance;
    }

    public override void TriggerAbility(Transform source)
    {
        GameObject explosionObj = Instantiate(explosion, pos.position, Quaternion.identity);
        explosionObj.GetComponent<SwordCollider>().Init(damage, source);
        camera.CameraShake(amplitude, frequency, time);
    }
}
