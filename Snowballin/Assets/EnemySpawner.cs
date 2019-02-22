using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int totalEnemiesAtOnce = 10;
    public Transform[] spawnPoints;
    public GameObject childPrefab;

    public GameObject[] waypoints;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemies", 2.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnEnemies(){
        var enemies = GameObject.FindGameObjectsWithTag("Target");
        var currentNumberEnemies = enemies == null? 0 : enemies.Length;
        while(currentNumberEnemies <= totalEnemiesAtOnce ){
            int theSpawnPoint = Random.Range(0, spawnPoints.Length);
            var child = Instantiate(childPrefab, spawnPoints[theSpawnPoint].position, spawnPoints[theSpawnPoint].rotation);
            var currentChl = child.GetComponent<Chidler>();
            currentChl.waypoints = waypoints;
            currentNumberEnemies++;
        }
    }
}
