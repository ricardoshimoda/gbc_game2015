using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidTracker : MonoBehaviour
{
    public static KidTracker instance;

    public List<GameObject> kidsAlive = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many KidTrackers!");
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if(kidsAlive.Count ==0)
        {
            //Debug.Log("All kids slain!");
        }
    }
}

