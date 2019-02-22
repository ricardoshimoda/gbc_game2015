using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chidler : MonoBehaviour
{
    public GameObject[] waypoints;
    public float runSpeed;
    public float waitTimeAtWaypoint;
    public float snowballShootFrequency;
    public float snowballSpeed;
    public GameObject snowballPrefab;
    public GameObject snowballSpawnPoint;

    private GameObject player;
    private GameObject currentWaypoint;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        SelectWaypoint();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToWaypoint();
    }

    void SelectWaypoint()
    {
        int index = Random.Range(0, waypoints.Length);
        currentWaypoint = waypoints[index];
        isMoving = true;
        transform.LookAt(currentWaypoint.transform);
        CancelInvoke("ShootSnowball");
    }

    void MoveToWaypoint()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.transform.position, runSpeed * Time.deltaTime);
            if (transform.position == currentWaypoint.transform.position)
            {
                transform.LookAt(player.transform);
                isMoving = false;
                Invoke("SelectWaypoint", waitTimeAtWaypoint);
                InvokeRepeating("ShootSnowball", 0, snowballShootFrequency);
            }
        }
    }

    void ShootSnowball()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                //Debug.Log("Player Visible");
                GameObject tempSnowball = Instantiate(snowballPrefab, snowballSpawnPoint.transform.position, snowballSpawnPoint.transform.rotation);
                tempSnowball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                tempSnowball.GetComponent<Rigidbody>().AddForce(transform.forward * snowballSpeed);
            }
            else
            {
                //Debug.Log("Player Not Visible");
            }
        }
    }
}
