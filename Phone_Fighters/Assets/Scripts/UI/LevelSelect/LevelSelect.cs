using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

    GameObject music;
    MainMenu mm;
    GameObject[] levels;

    private void Start()
    {
        GetStatistics();
        music = FindObjectOfType<AudioSource>().gameObject;
        mm = FindObjectOfType<MainMenu>();
        levels = GameObject.FindGameObjectsWithTag("Level");
    }

    public void LoadLevel(string name)
    {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("");
        PhotonNetwork.LoadLevel(name);
        Destroy(music);
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        if (result.Statistics.Count == 0)
        {
            UpdatePlayerStatistics();
        }
        else
        {
            foreach (var stat in result.Statistics)
            {
                Debug.Log("Statistic (" + stat.StatisticName + "): " + stat.Value);
                if (stat.StatisticName == "Progress")
                {
                    UnlockDoors(stat.Value);
                    return;
                }
            }
        }
    }

    void UnlockDoors(int progress)
    {
        foreach(GameObject obj in levels)
        {
            Level currentLevel = obj.GetComponent<Level>();
            if (currentLevel.ProgressNeeded <= progress)
            {
                currentLevel.OpenDoor();
            }
        }
        Debug.Log("unload");
        mm.UnloadScene();
    }

    void UpdatePlayerStatistics()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateStatistics",
            FunctionParameter = new {statisticName = "Progress", value = 0 },
            GeneratePlayStreamEvent = true
        }, OnCloudGameOverRewards => {
                                    Debug.Log("User statistics updated");
                                    UnlockDoors(0); }, 
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    //void UpdatePlayerStatistics()
    //{
    //    PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
    //            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
    //            Statistics = new List<StatisticUpdate> {
    //            new StatisticUpdate { StatisticName = "Progress", Value = 0 },
    //        }
    //    },
    //    result => {
    //        Debug.Log("User statistics updated");
    //        UnlockDoors(0);
    //    },
    //    error => { Debug.LogError(error.GenerateErrorReport()); });
    //}
}
