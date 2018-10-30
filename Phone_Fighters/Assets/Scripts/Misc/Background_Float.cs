using UnityEngine;

public class Background_Float : MonoBehaviour {

    Vector3 upper;
    Vector3 lower;
    bool goingDown;
    [SerializeField]
    float xMoveRange = 0, yMoveRange = 0, zMoveRange = 0, lerpAmount = 0;

    private void Start()
    {
        upper = transform.localPosition;
        lower = new Vector3(upper.x - xMoveRange, upper.y - yMoveRange, upper.z - zMoveRange);
        transform.localPosition = new Vector3(upper.x - (Random.Range(0, xMoveRange)), 
                                              upper.y - (Random.Range(0, yMoveRange)),
                                              upper.z - (Random.Range(0, zMoveRange)));
        goingDown = 0.5 > Random.Range(0, 1);
    }

    void Update () {
        Float();
	}

    void Float()
    {
        if (goingDown)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                lower,
                lerpAmount
                );
            if (transform.localPosition == lower)
            {
                goingDown = false;
            }
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                upper,
                lerpAmount
                );
            if (transform.localPosition == upper)
            {
                goingDown = true;
            }
        }
    }
}
