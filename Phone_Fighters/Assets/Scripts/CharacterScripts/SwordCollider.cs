using UnityEngine;

public class SwordCollider : MonoBehaviour {

    [SerializeField]
    string targetTag;
    public string TargetTag { get { return targetTag; } }

    int damage = 0;
    Transform source;
    public Transform Source { get { return source; } set { source = value; } }

    //[SerializeField]
    //bool knockBack = false;
    //public bool KnockBack { get { return knockBack; } }

    void Awake()
    {
        if (GameManager.Instance.IsPvp)
        {
            if (source.tag == "team1")
            {
                gameObject.tag = "DmgSource";
            }
            else if (source.tag == "team2")
            {
                gameObject.tag = "EnemyDmgSource";
            }
        }        
    }

    public void Init(int damage, Transform source)
    {
        this.damage = damage;
        this.source = source;
        this.damage += this.source.GetComponent<Character>().Damage;
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }
}
