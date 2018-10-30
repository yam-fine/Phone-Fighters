using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    bool isPvp = false;
    public bool IsPvp { get { return isPvp; } }
    static GameManager instance;
    //[SerializeField]
    //int maxPlayersInRoom = 2;
    [SerializeField]
    int progressToGive;
    public int ProgressToGive { get { return progressToGive; } }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
            return instance;
        }
    }
}
