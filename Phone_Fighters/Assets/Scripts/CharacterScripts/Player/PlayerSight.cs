using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour {

    List<Transform> nearbyEnemies = new List<Transform>();
    public List<Transform> NearbyEnemies {
        get { return nearbyEnemies; }
        set { nearbyEnemies = value; } 
    }
    GameManager gm;

	void Start ()
    {
        gm = GameManager.Instance;
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (gm.IsPvp)
        {

        }
        else
        {
            if (other.tag == "Enemy")
            {
                if (other.GetComponent<Enemy>().IsDead)
                    nearbyEnemies.Remove(other.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gm.IsPvp)
        {

        }
        else
        {
            if (other.tag == "Enemy")
            {
                nearbyEnemies.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gm.IsPvp)
        {

        }
        else
        {
            if (other.tag == "Enemy")
            {
                nearbyEnemies.Remove(other.transform);
            }
        }
    }
}
