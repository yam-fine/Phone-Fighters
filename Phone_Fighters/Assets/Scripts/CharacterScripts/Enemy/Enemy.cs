using UnityEngine;
using System.Collections;
using CorruptedSmileStudio.Spawn;

public delegate void OnDeath();

public class Enemy : Character {

    [SerializeField]
    int points;
    [SerializeField]
    int health;
    [SerializeField]
    float chaseSpeedMultiplier = 2f;
    public float ChaseSpeedMultiplier { get { return chaseSpeedMultiplier; } }
    protected float enemySpeed;
    public GameObject MyTarget { get; set; }
    [SerializeField]
    bool alwaysFollow = false;
    Score score;
    SpriteRenderer sr;
    public float PatrolTime { get; set; }
    public bool GoRight { get; set; }
    public bool DoIdle { get; set; }
    [SerializeField]
    Ability[] meleeAbilities;
    public Ability[] MeleeAbilities { get { return meleeAbilities; } }
    [SerializeField]
    Ability[] rangedAbilities;
    public Ability[] RangedAbilities { get { return rangedAbilities; } }
    [SerializeField]
    float attackRate = 3f;
    public float AttackRate { get { return attackRate; } set { attackRate = value; } }
    bool inRange = false;
    public bool InRange { get { return inRange; } set { inRange = value; } }
    public Ability CurrentAbility { get; set; }
    BoxCollider2D myColl;
    public event OnDeath OnDeath;
    [Tooltip("difference between the target's pivot point to this enemie's. for use in y axis")]
    [SerializeField]
    float fixFactor = 0;
    [SerializeField]
    bool knockedBack = true;
    [SerializeField]
    bool takeDmgAnimation = true;

    public override void Awake()
    {
        base.Awake();

        if (RangedAbilities.Length > 0)
            foreach (Ability ability in RangedAbilities)
                ability.Init();
        if (MeleeAbilities.Length > 0)
            foreach (Ability ability in MeleeAbilities)
                ability.Init();
    }

    public override void Start ()
    {
        base.Start();
        score = GameManager.Instance.GetComponent<Score>();
        enemySpeed = speed / 10f;
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        if (alwaysFollow)
        {
            MyTarget = Player.Instance.gameObject;
        }
        sr = GetComponent<SpriteRenderer>();
        myColl = GetComponent<BoxCollider2D>();
        //playerSight = MyTarget.GetComponentInChildren<PlayerSight>();
        //OnDeath += (() => playerSight.NearbyEnemies.Remove(transform));
    }

    public void RemoveTarget()
    {
        MyTarget = null;
    }

    [PunRPC]
    public override IEnumerator TakeDamage(int damage)
    {
        health -= damage;

        if (knockedBack)
            StartCoroutine(KnockBack());

        if (!IsDead)
        {
            if (takeDmgAnimation)
                MyAnimator.SetBool("TakeDmg", true);
        }
        else
        {
            score.CountScore = points;
            gameObject.GetComponent<SpawnAI>().Remove();
            MyAnimator.SetTrigger("Dead");
            yield return null;
        }
    }

    IEnumerator KnockBack()
    {
        MyRigidbody.AddForce(-transform.right * 300 * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        MyRigidbody.velocity = Vector2.zero;
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public bool AlwaysFollow
    {
        get
        {
            return alwaysFollow;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public void Move(Vector3 direction, float speed)
    {
        if (fixFactor != 0)
            direction = new Vector3(direction.x, direction.y + fixFactor);

        if (transform.position != direction && !Attack && !TakingDamage && !IsDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, speed);
            MyAnimator.SetFloat("Run", 1);
            LookAt(direction);
        }
    }

    void LookAt(Vector3 direction)
    {
        if (transform.position.x < direction.x)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
    }

    public void ResetAttack(string collider)
    {
        Collider2D edgeCollider = transform.Find(collider).GetComponent<Collider2D>();
        edgeCollider.enabled = false;
    }

    public override void Death()
    {
        myColl.enabled = false;
        OnDeath();
        StartCoroutine(DeathEffects());
    }

    IEnumerator DeathEffects()
    {
        float time = 0;
        while (time < 1f)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(.1f);
            time += .2f;
        }
        InstanceManager.Despawn(transform);
    }

    public void Patrol()
    {
        if (GoRight)
        {
            transform.Translate(-Vector2.right * Speed * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
    }

    public bool IsFacingRight()
    {
        if (transform.rotation.y == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MeleeAbility()
    {
        CurrentAbility = meleeAbilities[Random.Range(0, meleeAbilities.Length - 1)];
        MyAnimator.SetBool(CurrentAbility.AnimatorParamName, true);
    }

    public void RangedAbility()
    {
        CurrentAbility = rangedAbilities[Random.Range(0, rangedAbilities.Length - 1)];
        MyAnimator.SetBool(CurrentAbility.AnimatorParamName, true);
    }

    public void Ability()
    {
        CurrentAbility.TriggerAbility(transform);
    }
}
