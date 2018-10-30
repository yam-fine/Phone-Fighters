using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class GameOver : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI coinsEarned;
    [SerializeField]
    Score score;
    int coinReward = 0;
    GameManager gm;

    void Start()
    {
        Debug.Log("Game Over");
        gm = GameManager.Instance;

        switch (gm.GetComponent<StageManager>().DifficultyLevel)
        {
            case CorruptedSmileStudio.Spawn.UnitLevels.Easy:
                coinReward = score.CountScore;
                break;
            case CorruptedSmileStudio.Spawn.UnitLevels.Medium:
                coinReward = Mathf.RoundToInt(score.CountScore * 1.5f);
                break;
            case CorruptedSmileStudio.Spawn.UnitLevels.Hard:
                coinReward = Mathf.RoundToInt(score.CountScore * 2f);
                break;
            case CorruptedSmileStudio.Spawn.UnitLevels.Boss:
                coinReward = Mathf.RoundToInt(score.CountScore * 3f);
                break;
        }        
        coinsEarned.text = coinReward.ToString();
        CloudGrantCoinReward(coinReward);
        UpdatePlayerProgress(gm.ProgressToGive);
    }

    void UpdatePlayerProgress(int progress)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateStatistics",
            FunctionParameter = new { statisticName = "Progress", value = progress },
            GeneratePlayStreamEvent = true
        }, OnCloudGameOverRewards => {
            Debug.Log("User statistics updated");
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    void CloudGrantCoinReward(int coinAmount)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GameOverRewards",
            FunctionParameter = new { coinAmount = coinAmount},
            GeneratePlayStreamEvent = true
        }, OnCloudGameOverRewards, OnErrorShared);
    }

    void OnCloudGameOverRewards(ExecuteCloudScriptResult result)
    {
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
    }

    void OnErrorShared(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
