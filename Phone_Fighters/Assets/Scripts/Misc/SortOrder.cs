using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortOrder : MonoBehaviour {

    List<SpriteRenderer> spriteRenderers;
    SpriteRenderer mySP;

	// Use this for initialization
	void Awake ()
    {
        mySP = GetComponent<SpriteRenderer>();
        spriteRenderers = new List<SpriteRenderer>(GetComponentsInParent<SpriteRenderer>());
        spriteRenderers.Remove(mySP);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Sort();
	}

    void Sort()
    {
        int pos = Mathf.RoundToInt(transform.position.y * 100);
        pos /= 3;

        if (spriteRenderers.Count == 1)
            SingleSP(pos);
        else
            ManySP(pos);
    }

    void SingleSP(int pos)
    {
        mySP.sortingOrder = (pos + 1) * -1;
        spriteRenderers[0].sortingOrder = pos * -1;
    }

    void ManySP(int pos)
    {
        mySP.sortingOrder = (pos + 1) * -1;
        foreach (SpriteRenderer sp in spriteRenderers)
        {
            sp.sortingOrder = pos * -1;
        }
    }
}
