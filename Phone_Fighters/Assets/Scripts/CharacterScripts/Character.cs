using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

    public Animator MyAnimator { get; private set; }
    public Rigidbody2D MyRigidbody { get; private set; }
    protected bool canAttack;
    [SerializeField]
    protected int damage;
    public int Damage { get { return damage; } set { damage = value; } }    
    [SerializeField]
    protected int armor = 1;
    public int Armor { get { return armor; } set { armor = value; } }
    [SerializeField]
    protected float speed = 1f;
    public float Speed { get { return speed; } }
    [SerializeField]
    protected List<string> takeDamageSourcesTags;
    public bool Attack { get; set; }
    public abstract IEnumerator TakeDamage(int damage);
    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }
    public abstract void Death();
    protected static GameManager gm;

    public virtual void Awake()
    {
        MyAnimator = GetComponent<Animator>();
        MyRigidbody = GetComponent<Rigidbody2D>();
        damage = Mathf.Clamp(damage, 1, 99);
        armor = Mathf.Clamp(armor, 1, 99);
        gm = GameManager.Instance;
    }

    public virtual void Start ()
    {
    }

    public virtual void Update ()
    {
        
	}
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (takeDamageSourcesTags.Contains(other.tag))
        {
            GetComponent<PhotonView>().RPC("TakeDamage", 
                                          PhotonTargets.AllBuffered, 
                                          other.GetComponent<SwordCollider>().Damage / armor);
        }
    }
}
