using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {

    int score = 0;
    public int CountScore { get { return score; } set { score += value; } }
    [SerializeField]
    TextMeshProUGUI scoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = score.ToString();	
	}
}
