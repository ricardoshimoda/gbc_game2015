using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Solution : MonoBehaviour
{
    public static Level1Solution instance;

    GameObject chandalier;
    GameObject playerTable;
    GameObject dynamite;
    GameObject bowlingBall;
    GameObject knives;
    GameObject piano;
    ChandalierScript cs;

    private void Awake()
    {
        if(instance!= null)
        {
            Debug.Log("Too many solutions!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        chandalier = GameObject.Find("Chandelier");
        cs = chandalier.GetComponent<ChandalierScript>();
    }

    public void DropC()
    {
        cs.Drop();
    }
}
