using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CorruptedSmileStudio.Spawn;

public class StageManager : MonoBehaviour {

    [SerializeField]
    List<ArrayOfGameObject> stageList = new List<ArrayOfGameObject>();
    [SerializeField]
    UnitLevels difficultyLevel;
    public UnitLevels DifficultyLevel { get { return difficultyLevel; } }
    [SerializeField]
    GameObject gameOverUI;
    AudioSource musicBox;
    ArrayOfGameObject currentStage;
    //bool gameOver = false;
    int stageCount = 0;
    WaitForSeconds one = new WaitForSeconds(1);

    void Start ()
    {
        musicBox = GameObject.FindGameObjectWithTag("MusicBox").GetComponent<AudioSource>();
        musicBox.clip = stageList[0].Audio;
        currentStage = stageList[stageCount];
        foreach (ArrayOfGameObject wave in stageList)
        {
            foreach (GameObject spawner in wave)
            {
                spawner.GetComponent<Spawner>().UnitLevel = difficultyLevel;
            }
        }

        StartCoroutine(StageController());
    }

    IEnumerator StageController()
    {
        int count = 0;
        WaveStart();

        // loop through stages
        while (stageCount < stageList.Count)
        {
            foreach (GameObject spawnerObj in currentStage)
            {
                Spawner spawner = spawnerObj.GetComponent<Spawner>();
                if (spawner.spawn || spawner.numberOfUnits != 0)
                    break;
                count++;
            }
            if (count == currentStage.Count)
                WaveStart();

            count = 0;
            yield return one;
        }

        // wait for final stage to clear
        bool EnemiesLeft = true;
        while (EnemiesLeft)
        {
            foreach (GameObject spawnerObj in currentStage)
            {
                Spawner spawner = spawnerObj.GetComponent<Spawner>();
                if (spawner.spawn || spawner.numberOfUnits != 0)
                    break;
                count++;
            }
            if (count == currentStage.Count)
                EnemiesLeft = false;

            count = 0;
            yield return one;
        }
        GameOver();        
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    private void WaveStart()
    {
        Debug.Log("current wave is: " + (stageCount + 1));
        currentStage = stageList[stageCount];
        musicBox.clip = currentStage.Audio;

        foreach (GameObject spawnerObj in currentStage)
        {
            Spawner spawner = spawnerObj.GetComponent<Spawner>();
            spawner.EnableSpawner(spawner.spawnID);
        }

        stageCount++;
    }
}
