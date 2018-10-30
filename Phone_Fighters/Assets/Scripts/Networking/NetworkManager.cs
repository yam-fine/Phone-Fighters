using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {

    [SerializeField]
    GameObject standbyCamera;
    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    List<GameObject> virtualCamera;
    [SerializeField]
    string charPrefabName;
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>(2);
    Transform spawnPoint;
    GameObject myPlayerGO;
    [SerializeField]
    GameObject ui;
    static NetworkManager instance;
    bool matchStarted = false;
    GameObject myPlayer;
    public bool MatchStarted { get { return matchStarted; } }

    void Start ()
    {
        if (!PhotonNetwork.offlineMode)
        {
            Connect();
        }
        else
        {
            SpawnPoint(0);
            SpawnPlayer();
        }
	}

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("1.0.0");      
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinLobby();
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int team = 0;
            SpawnPoint(team);
        }
        else
        {
            int team = 1;
            SpawnPoint(team);
        }
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (standbyCamera != null)
        {
            standbyCamera.SetActive(false);
        }
        Initialize();
    }

    void SpawnPoint(int team)
    {
        if (spawnPoints.Count == 1)
        {
            spawnPoint = spawnPoints[0];
        }
        else
        {
            spawnPoint = spawnPoints[team];
        }
    }

    void Initialize()
    {
        Debug.Log("initialize");
        myPlayerGO = PhotonNetwork.Instantiate(charPrefabName,
                                               spawnPoint.position,
                                               spawnPoint.rotation,
                                               0); 
        myPlayerGO.GetComponent<InputManager>().enabled = true;
        foreach (GameObject cam in virtualCamera) {
            cam.GetComponent<CinemachineVirtualCamera>().Follow = myPlayerGO.transform;
            cam.SetActive(true);
            mainCamera.SetActive(true);
        }
        PhotonView pv = myPlayerGO.GetComponent<PhotonView>();
        ui.SetActive(true);

        if (GameManager.Instance.IsPvp)
        {            
            if (PhotonNetwork.isMasterClient)
            {
                pv.RPC("Set_Team", PhotonTargets.AllBuffered, "team1");
            }
            else
            {
                pv.RPC("Set_Team", PhotonTargets.AllBuffered, "team2");
            }
            pv.RPC("Set_Tags", PhotonTargets.AllBuffered);
        }
        else // game is PvE
        {
            //GameManager.Instance.GetComponent<WaveManager>().enabled = true;
            GameManager.Instance.GetComponent<StageManager>().enabled = true;
        }
        pv.RPC("Set_UI", PhotonTargets.AllBuffered);
    }

    public GameObject MyPlayer
    {
        get
        {
            return myPlayerGO;
        }
    }

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
            }
            return instance;
        }
    }
    
    IEnumerator StartMatch()
    {
        Debug.Log("starting wait");
        yield return new WaitUntil(() => PhotonNetwork.playerList.Length >= 2);

        Debug.Log("finished waiting");
        //do animation
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("team1").Length >= 2);
        Debug.Log("all players spawned");

        yield return new WaitForSeconds(3);

        Debug.Log("match started!");
    }    
}
