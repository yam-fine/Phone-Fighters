using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void DeadEventHandler();
public delegate void ResetBasicAttack();
public delegate void TriggerAbility();

public class Player : Character {

    // abilities
    [SerializeField]
    List<Ability> abilities = new List<Ability>();
    public List<Ability> Abilities { get { return abilities; } }
    public event TriggerAbility OnAbility0;
    public event TriggerAbility OnAbility1;
    public event TriggerAbility OnAbility2;
    public event TriggerAbility OnAbility3;

    [SerializeField]
    float healthRegen = 1f, staminaRegen = 1f, startRegen = 1f;
    bool canRegen = true;
    [SerializeField]
    Stat healthBar, staminaBar;
    public BarHandler HealthBar { get { return healthBar.Bar; } set { healthBar.Bar = value; } }
    public BarHandler StaminaBar { get { return staminaBar.Bar; } set { staminaBar.Bar = value; } }
    public float Stamina { get { return staminaBar.CurrentValue; } }
    bool immortal = false;
    public bool Immortal { get { return immortal; } }
    [SerializeField]
    float immortalTime;
    public event DeadEventHandler Dead;
    static Player instance;
    Vector3 temPos;
    BoxCollider2D boxCol;
    SpriteRenderer sr;
    static NetworkManager nm;
    float lastTimeTakenDmg = 0;
    public List<string> TakeDamageSourcesTags { get { return takeDamageSourcesTags; } set { takeDamageSourcesTags = value; } }

    // Audio
    AudioSource myVoice;
    AudioClip abilitySound;
    public AudioClip AbilitySound { get { return abilitySound; } set { abilitySound = value; } }

    // ignore collision    
    [SerializeField]
    List<Collider2D> enemySightShouldIgnoreColl = new List<Collider2D>();
    public List<Collider2D> EnemySightShouldIgnoreColl { get { return enemySightShouldIgnoreColl; } }


    // Player Sight
    PlayerSight sight;
    Transform nearestTarget = null;
    float closestDistanceSqr = Mathf.Infinity;
    float maxCloseDistance = 100f;
    float minCloseDistance = 10f;
    float speedCloseDistance = 1000f;

    public static event ResetBasicAttack OnResetAttack;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start ()
    {
        base.Start();
        if (gm.IsPvp)
        {
            nm = NetworkManager.Instance;
        }

        sr = GetComponent<SpriteRenderer>();
        healthBar.Initialize();
        staminaBar.Initialize();
        boxCol = GetComponent<BoxCollider2D>();        
        sight = GetComponentInChildren<PlayerSight>();
        myVoice = GetComponent<AudioSource>();

        Dead += (() => MyAnimator.SetTrigger("Dead"));
        Dead += (() => boxCol.enabled = false);
    }

    [PunRPC]
    public override IEnumerator TakeDamage(int dmg)
    {
        if (!immortal)
        {
            healthBar.CurrentValue -= dmg;
            canRegen = false;
            lastTimeTakenDmg = 0;

            if (!IsDead && !gm.IsPvp)
            {
                immortal = true;
                StartCoroutine(IndicateImmortal());
                if (!Attack)
                    MyAnimator.SetBool("TakeDmg", true);
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                if (Dead != null)
                {
                    Dead();
                }
            }
        }
    }

    IEnumerator IndicateImmortal()
    {
        while (immortal)
        {            
            sr.enabled = false;
            yield return new WaitForSeconds(.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(.1f);
        }        
    }

    public override void Update ()
    {
        base.Update();
        if (!canRegen)
        {
            lastTimeTakenDmg += Time.deltaTime;
            CanRegen();
        }
        else if (!IsDead && !TakingDamage && canRegen)
        {
            Regen();
        }
    }

    void CanRegen()
    {        
        if (lastTimeTakenDmg >= startRegen)
        {
            canRegen = true;
            lastTimeTakenDmg = 0;
        }
    }

    void Regen()
    {
        if (healthBar.CurrentValue < healthBar.MaxValue)
            healthBar.CurrentValue += healthRegen * Time.deltaTime;
        if (staminaBar.CurrentValue < staminaBar.MaxValue)
            staminaBar.CurrentValue += staminaRegen * Time.deltaTime;
    }

    public override bool IsDead
    {
        get
        {
            return healthBar.CurrentValue <= 0;
        }
    }

    public static Player Instance
    {
        get
        {
            if (instance == null && !gm.IsPvp)
            {
                instance = FindObjectOfType<Player>();
            }
            else if (instance == null && gm.IsPvp)
            {
                instance = nm.MyPlayer.GetComponent<Player>();
            }
            return instance;
        }
    }

    public override void Death() // runs in death behaviour
    {       
        Revive();

        // make all enemies set to "always follow" see me again
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            if (enem.AlwaysFollow)
            {
                enem.MyTarget = gameObject;
            }
        }
    }

    void Revive()
    {
        MyRigidbody.velocity = Vector2.zero;
        healthBar.CurrentValue = healthBar.MaxValue;
        canRegen = true;

        //To trigger the OnTriggerEnter
        boxCol.enabled = true;
    }

    public void LowerStamina(float value)
    {
        if (staminaBar.CurrentValue >= value)
        {
            staminaBar.CurrentValue -= value;
            canRegen = false;
        }
    }

    Transform FindNearest(List<Transform> enemies)
    {
        foreach (Transform potentialTarget in enemies)
        {
            Vector2 directionToTarget = potentialTarget.position - transform.position; // enemy pos - my pos
            float dSqrToTarget = directionToTarget.sqrMagnitude; // using square distance as its more efficient than vector3.dist
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestTarget = potentialTarget;
            }
        }

        return nearestTarget; // returns null if no enemy in sight collider
    }

    public void MoveToNearest()
    {
        Vector2 directionToTarget;
        Transform nearestEnemy;

        if (sight.NearbyEnemies.Count == 0 || sight.NearbyEnemies == null)
        {
            StartCoroutine(CloseDistance(transform.right, 400f, 0.2f));
            return; // no need to continue
        } 
        else if (sight.NearbyEnemies.Count == 1)
        {
            nearestEnemy = sight.NearbyEnemies[0];            
        }
        else
        {
            nearestEnemy = FindNearest(sight.NearbyEnemies);
        }
        directionToTarget = nearestEnemy.position - transform.position;

        // distance to enemy is larger than min close distance
        if (directionToTarget.sqrMagnitude > minCloseDistance) 
        {
            ChangeRotation(nearestEnemy.position);

            // between max range and min range
            if (directionToTarget.sqrMagnitude <= maxCloseDistance + minCloseDistance && directionToTarget.sqrMagnitude > minCloseDistance)
            {
                StartCoroutine(CloseDistance(directionToTarget, 
                                             speedCloseDistance, 
                                             (directionToTarget.sqrMagnitude / speedCloseDistance))); // distance should be counted with Mathf.Sqrt
            }
            //higher than max range
            else
            {
                StartCoroutine(CloseDistance(directionToTarget, 
                                             speedCloseDistance, 
                                             (maxCloseDistance / speedCloseDistance))); // distance should be counted with Mathf.Sqrt
            }
        }
    }

    public IEnumerator CloseDistance(Vector2 direction, float speed, float time)
    {
        MyRigidbody.AddForce(direction.normalized * speed * Time.deltaTime, ForceMode2D.Impulse);
        MyAnimator.enabled = false;
        yield return new WaitForSeconds(time);

        MyAnimator.enabled = true;
        MyRigidbody.velocity = Vector2.zero;
    }

    public void ChangeRotation(Vector3 enemy)
    {
        if (enemy.x == transform.position.x)
            return;// nothing
        else if (enemy.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else // <
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void ResetAttack(string collider)
    {
        Collider2D edgeCollider = transform.Find(collider).GetComponent<Collider2D>();
        edgeCollider.enabled = false;

        if (OnResetAttack != null)
            OnResetAttack();
    }

    public void Ability0()
    {
        if (OnAbility0 != null)
            OnAbility0();
    }

    public void Ability1()
    {
        if (OnAbility1 != null)
            OnAbility1();
    }

    public void Ability2()
    {
        if (OnAbility2 != null)
            OnAbility2();
    }

    public void Ability3()
    {
        if (OnAbility3 != null)
            OnAbility3();
    }

    public void TriggerAudio()
    {
        myVoice.clip = abilitySound;
        myVoice.Play();
    }
}
