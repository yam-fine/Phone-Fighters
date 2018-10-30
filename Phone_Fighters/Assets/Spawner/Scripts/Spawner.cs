using UnityEngine;
using CorruptedSmileStudio.Spawn;

/// <summary>
/// Spawns prefabs, either in waves, at once or continually till all enemies are spawned.
/// </summary>
/// <description>
/// Controls the spawning of selected perfabs, useful for making enemy spawn points.<br />
/// It supports a variety of spawn modes, which allows you to bend the system to fit your needs.<br />
/// This class is required for the system to work, you will need to place this class on a GameObject with
/// a tag of Spawner (However, this is changeable within the SpawnAI class).
/// </description>
public class Spawner : MonoBehaviour
{
    UnitLevels unitLevel = UnitLevels.Easy;
    public UnitLevels UnitLevel { get { return unitLevel; } set { unitLevel = value; } }
    public GameObject[] unitList = new GameObject[4];
    public int totalUnits = 5;
    public int numberOfUnits = 0;
    int totalSpawnedUnits = 0;
	public int spawnID = 0;
    bool waveSpawn = true;
    public bool spawn = true;
    public SpawnModes spawnType = SpawnModes.Normal;
    public float waveTimer = 30.0f;
    float lastWaveSpawnTime = 0.0f;
    public int totalWaves = 1;
    int numWaves = 0;
    bool checkEnemyLevel = false;
    public float timeBetweenSpawns = 0.5f;
    public Transform[] spawnPoints;
    int spawnPointIndex;

    void Start()
    {
        InstanceManager.ReadyPreSpawn(unitList[(int)unitLevel].transform, totalUnits);
        StartCoroutine("DoSpawn");
    }
    /// <summary>
    /// Spawns a unit based on the level set.
    /// </summary>
    private void SpawnUnit()
    {
        if (unitList[(int)unitLevel] != null)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform unit = InstanceManager.Spawn(unitList[(int)unitLevel].transform, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            unit.GetComponent<SpawnAI>().SetOwner(this);
            // Increase the total number of enemies spawned and the number of spawned enemies
            numberOfUnits++;
            totalSpawnedUnits++;
        }
        else
        {
            Debug.LogError("Error trying to spawn unit of level " + unitLevel.ToString() + " on spawner " + spawnID + " - No unit set");
            spawn = false;
        }
    }
    /// <summary>
    /// This removes an "unit" in order to allow waves and such that depend on the number of enemies decreasing
    /// to properly start a new spawn.
    /// </summary>
    /// <param name="sID">The spawner ID that created the unit.</param>
    public void RemoveUnit(int sID)
    {
        // if the unit's spawnID is equal to this spawnersID then remove an unit count
        if (spawnID == sID)
        {
            numberOfUnits--;
        }
    }
	/// <summary>
	/// This removes an "unit" in order to allow waves and such that depend on the number of enemies decreasing
	/// to properly start a new spawn.
	/// </summary>
	public void RemoveUnit()
	{
		numberOfUnits--;
	}
    /// <summary>
    /// Enable the spawner by ID
    /// </summary>
    /// <param name="sID">The spawner's ID</param>
    public void EnableSpawner(int sID)
    {
        if (spawnID == sID)
        {
            spawn = true;
            InstanceManager.ReadyPreSpawn(unitList[(int)unitLevel].transform, totalUnits);
            StartCoroutine("DoSpawn");
        }
    }
    /// <summary>
    /// Disable the spawner by ID
    /// </summary>
    /// <param name="sID">The spawner's ID</param>
    public void DisableSpawner(int sID)
    {
        if (spawnID == sID)
        {
            spawn = false;
            StopCoroutine("DoSpawn");
        }
    }
    /// <summary>
    /// The time till the next wave
    /// </summary>
    public float TimeTillWave
    {
        get
        {
            if (numWaves >= totalWaves)
            {
                return 0;
            }
            if (spawnType == SpawnModes.TimeSplitWave && waveSpawn || numberOfUnits > 0)
            {
                return 0;
            }

            float time = (lastWaveSpawnTime + waveTimer) - Time.time;
            if (time >= 0)
                return time;
            else
                return 0;
        }
    }
    /// <summary>
    /// Enables the spawner. Useful for triggers.
    /// </summary>
    public void EnableViaTrigger()
    {
        spawn = true;
        StartCoroutine("DoSpawn");
    }
    /// <summary>
    /// Draws an icon to show where the spawn point is. Useful if you don't have a object that show the spawn point
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "SpawnIcon.psd");
    }
    /// <summary>
    /// Resets all the private variables to their original state.
    /// </summary>
    public void Reset()
    {
        waveSpawn = false;
        checkEnemyLevel = true;
        numWaves = 0;
        totalSpawnedUnits = 0;
        lastWaveSpawnTime = Time.time;
    }
    /// <summary>
    /// Returns the number of waves left
    /// </summary>
    public int WavesLeft
    {
        get
        {
            return totalWaves - numWaves;
        }
    }
    /// <summary>
    /// Controls the spawning of units and so forth.
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator DoSpawn()
    {
        while (spawn)
        {
            switch (spawnType)
            {
                case SpawnModes.Normal:
                    if (numberOfUnits < totalUnits)
                    {
                        yield return new WaitForSeconds(timeBetweenSpawns);
                        SpawnUnit();
                    }
                    break;
                case SpawnModes.Once:
                    if (totalSpawnedUnits >= totalUnits)
                    {
                        spawn = false;
                    }
                    else
                    {
                        yield return new WaitForSeconds(timeBetweenSpawns);
                        SpawnUnit();
                    }
                    break;
                case SpawnModes.Wave:
                    if (numWaves <= totalWaves)
                    {
                        if (waveSpawn)
                        {
                            yield return new WaitForSeconds(timeBetweenSpawns);
                            SpawnUnit();
                            checkEnemyLevel = true;

                            if ((totalSpawnedUnits / (numWaves + 1)) == totalUnits)
                            {
                                waveSpawn = false;
                            }
                        }
                        if (checkEnemyLevel)
                        {
                            if (numberOfUnits <= 0)
                            {
                                checkEnemyLevel = false;
                                waveSpawn = true;
                                numberOfUnits = 0;
                                numWaves++;
                            }
                        }
                    }
                    else
                    {
                        spawn = false;
                    }
                    break;
                case SpawnModes.TimedWave:
                    if (numWaves <= totalWaves)
                    {
                        if (waveSpawn)
                        {
                            yield return new WaitForSeconds(timeBetweenSpawns);
                            SpawnUnit();

                            if ((totalSpawnedUnits / (numWaves + 1)) == totalUnits)
                            {
                                waveSpawn = false;
                            }
                        }
                        else
                        {
                            waveSpawn = true;
                            numWaves++;
                            // A hack to spawn even if there are unit left alive.
                            numberOfUnits = 0;
                            lastWaveSpawnTime = Time.time;
                            yield return new WaitForSeconds(waveTimer);
                        }
                    }
                    else
                    {
                        spawn = false;
                    }
                    break;
                case SpawnModes.TimeSplitWave:
                    if (numWaves <= totalWaves)
                    {
                        if (waveSpawn)
                        {
                            yield return new WaitForSeconds(timeBetweenSpawns);
                            SpawnUnit();
                            if ((totalSpawnedUnits / (numWaves + 1)) == totalUnits)
                            {
                                waveSpawn = false;
                                checkEnemyLevel = true;
                            }
                        }
                        else
                        {
                            if (checkEnemyLevel)
                            {
                                if (numberOfUnits <= 0)
                                {
                                    numWaves++;
                                    checkEnemyLevel = false;
                                    numberOfUnits = 0;
                                    lastWaveSpawnTime = Time.time;
                                    yield return new WaitForSeconds(waveTimer);
                                    waveSpawn = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        spawn = false;
                    }
                    break;
                default:
                    spawn = false;
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    /// <summary>
    /// Starts spawning, if you want to interact with a spawner from code.
    /// </summary>
    public void StartSpawn()
    {
        EnableViaTrigger();
    }

    public float WaveTimer
    {
        get
        {
            return waveTimer;
        }
    }
}