using UnityEngine;

public abstract class Ability : ScriptableObject {

    [SerializeField]
    protected int damage = 0;
    [SerializeField]
    protected AudioClip[] sound;
    public AudioClip Sound { get { return sound[UnityEngine.Random.Range(0, sound.Length)]; } }
    [SerializeField]
    protected float coolDown = 0;
    public float CoolDown { get { return coolDown; } }
    [SerializeField]
    protected Sprite icon;
    public Sprite Icon { get { return icon; } }
    [SerializeField]
    protected string animatorParamName;
    public string AnimatorParamName { get { return animatorParamName; } }

    public abstract void Init();
    public abstract void TriggerAbility(Transform source);
}
