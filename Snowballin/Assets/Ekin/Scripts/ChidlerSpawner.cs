using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChidlerSpawner : MonoBehaviour
{
    public GameObject spawnPoint;           // Spawn point of the chidlers.
    public GameObject chidlerPrefab;        // Prefab for the chidlers (duh).
    public int initialWaveSize;             // Number of chidlers in the first wave.
    public int finalWaveSize;               // Number of chidlers in the last wave.
    public int spawnFrequency;              // Seconds it takes to spawn the next chidler.
    public int timeBetweenWaves;            // Time it takes before the next wave starts.

    private List<int> chidlerIds;
    private int waveSize;
    private bool levelInProgress;
    private bool spawnInProgress;
    private bool waveInProgress;
    private int chidlerCount;
    private GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        levelInProgress = true;
        waveSize = initialWaveSize;
        chidlerCount = 0;
        waveInProgress = false;
        spawnInProgress = false;
    }
    
    void Update()
    {
        if (levelInProgress)
        {
            SpawnWave();
            CheckChidlerCount();
        }
    }

    void SpawnChidler()
    {
        GameObject tempChidler = Instantiate(chidlerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        tempChidler.GetComponent<Chidler>().SetId(GetId());
        chidlerCount++;
        if (chidlerCount == waveSize)
        {
            spawnInProgress = false;
            CancelInvoke("SpawnChidler");
        }
    }

    void SpawnWave()
    {
        if (!spawnInProgress && !waveInProgress)
        {
            ResetIdList();
            spawnInProgress = true;
            waveInProgress = true;
            InvokeRepeating("SpawnChidler", timeBetweenWaves, spawnFrequency);
        }
    }

    void CheckChidlerCount()
    {
        GameObject[] chidlers = GameObject.FindGameObjectsWithTag("Target");
        if (chidlers.Length == 0 && !spawnInProgress)
        {
            if (waveSize != finalWaveSize)
            {
                player.GetComponent<PlayerController>().WaveComplete(waveSize - initialWaveSize + 1);
            }
            waveInProgress = false;
            waveSize++;
            chidlerCount = 0;
            if (waveSize > finalWaveSize)
            {
                levelInProgress = false;
                player.GetComponent<PlayerController>().LevelComplete();
            }
        }
    }

    void ResetIdList()
    {
        chidlerIds = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            chidlerIds.Add(i);
        }
    }

    int GetId()
    {
        int randomIndex = Random.Range(0, chidlerIds.Count);
        int id = chidlerIds[randomIndex];
        chidlerIds.Remove(randomIndex);
        return id;
    }
}