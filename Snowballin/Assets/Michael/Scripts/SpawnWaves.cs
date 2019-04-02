using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    public static int numOfEnemies = 0;
    int maxEnemies = 6;
    [SerializeField] GameObject Enemies;
    float time = 0.0f;
  

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Timer());

        /*if(time % 30 == 0)
        {
            maxEnemies += 5;
        }*/
    }
    
    void Spawn()
    {
        if(numOfEnemies < maxEnemies)
        {
            GameObject.Instantiate(Enemies, this.transform.position, this.transform.rotation);
            numOfEnemies++;

            if (numOfEnemies == 0)
            {
                maxEnemies += 5;
            }
           // Debug.Log("Spawn");
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1.0f);
        Spawn();
        time++;
        Debug.Log(time);
       
    }
}
