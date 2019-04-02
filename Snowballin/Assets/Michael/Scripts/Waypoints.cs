using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] protected float radius = 50.0f;

    List<Waypoints> connections;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] waypointsArray = GameObject.FindGameObjectsWithTag("Waypoint");
        connections = new List<Waypoints>();

        for (int i = 0; i < waypointsArray.Length; i++)
        {
            Waypoints nextWaypoint = waypointsArray[i].GetComponent<Waypoints>();

            if(nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= radius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public Waypoints FindNextWaypoint(Waypoints previousWaypoint)
    {
        if (connections.Count == 0) 
        {
            Debug.Log("There is no waypoint");
            return null;
        }
        else if (connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        else
        {
            Waypoints nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];

            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
