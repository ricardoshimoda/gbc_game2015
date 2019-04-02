using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Material red, green;
    public bool behindCover;

    private GameObject debugCube;
    private bool isOccupied; // True: A chidler is occupying this waypoint or is on his/her way to this waypoint. False: Waypoint is not occupied.
    
    void Start()
    {
        isOccupied = false;
        debugCube = transform.Find("DebugCube").gameObject;
        if (debugCube != null)
        {
            debugCube.GetComponent<MeshRenderer>().material = green;
        }
    }

    public void SetOccupied(bool flag)
    {
        isOccupied = flag;
        if (debugCube != null)
        {
            if (isOccupied)
            {
                debugCube.GetComponent<MeshRenderer>().material = red;
            }
            else
            {
                debugCube.GetComponent<MeshRenderer>().material = green;
            }
        }
    }

    public bool GetOccupied()
    {
        return isOccupied;    
    }
}
