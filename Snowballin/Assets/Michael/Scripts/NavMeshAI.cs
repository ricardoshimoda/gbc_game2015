using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAI : MonoBehaviour
{
    //[SerializeField] Transform _destination;
    [SerializeField] bool patrolWaiting;
    [SerializeField] float totalWaitTime = 5.0f;

    Waypoints currentWaypoint;
    Waypoints previousWaypoint;
    NavMeshAgent navMeshAgent;
    public float snowballSpeed;
    public float speed = 5.0f;
    bool isRunning;
    bool isHiding;
    float waitTime;
    int waypointVisited;
    
    public float rangeToPlayer;
    public GameObject bullet;
    public GameObject spawn;
    public Transform eyes;
    private GameObject player;
    private bool firing = false;
    private float fireTime;
    private float coolDown = 3.0F;
    Animator animator;



    public float maxAngle;
    public float maxRadius;

    private bool isInFov = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawn.transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, eyes.up) * eyes.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, eyes.up) * eyes.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(eyes.position, fovLine1);
        Gizmos.DrawRay(eyes.position, fovLine2);

        if (!isInFov)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(eyes.position, (player.transform.position - eyes.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(eyes.position, eyes.forward * maxRadius);


    }
    // Use this for initialization
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        animator.SetInteger("State", 0);
    }

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        speed = UnityEngine.Random.Range(2.0f, 3.0f);
        navMeshAgent.speed = speed;
        player = GameObject.FindGameObjectWithTag("Target");
        if (navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            if (currentWaypoint == null)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        Waypoints startWaypoit = allWaypoints[random].GetComponent<Waypoints>();

                        if (startWaypoit != null)
                        {
                            currentWaypoint = startWaypoit;
                        }
                    }
                }
            }
            else Debug.Log("There is no waypoint");
        }
        SetDestination();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Target");
       // OnDrawGizmos();
       // Debug.Log(inFOV(eyes, player.transform, maxAngle, maxRadius));
        if (navMeshAgent.speed != 0) animator.SetInteger("State", 1);
        else
        {
           if (!firing) animator.SetInteger("State", 0);
        }

        int rand = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f, 3.0f));
       if ( InFOV(eyes, player.transform, maxAngle, maxRadius) && rand == 0)  
        {
            // coolDown = 0.0f;
            navMeshAgent.speed = 0;
            Shooting();
        }

        if(isRunning && navMeshAgent.remainingDistance <= 1.0f)
        {
            
           if(PlayerInRange())
            {
                //transform.LookAt(player.transform);
                lookAtPlayer();
                Shooting();
            };

            isRunning = false;
            waypointVisited++;

            if(patrolWaiting)
            {
                isHiding = true;
                waitTime = 0.0f;
            }
            else
            {
                SetDestination();
            }
        }

        if(isHiding)
        {
          
            if (PlayerInRange())
            {
                //transform.LookAt(player.transform);
                lookAtPlayer();
                Shooting();
            };
            waitTime += Time.deltaTime;

            if(waitTime >= totalWaitTime)
            {
                isHiding = false;
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if (waypointVisited > 0)
        {
            Waypoints nextWaypoint = currentWaypoint.FindNextWaypoint(previousWaypoint);
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
        }

        Vector3 target = currentWaypoint.transform.position;
       // animator.SetInteger("State", 1);
        navMeshAgent.SetDestination(target);

        isRunning = true;
    }

    void Shooting()
    {
        if (PlayerInRange())
        {
            //Debug.Log("Target");
            RaycastHit hit;

           // if (Physics.Raycast(spawn.transform.position, transform.TransformDirection(Vector3.forward), out hit))
         //  {
               // Debug.DrawRay(spawn.transform.position, transform.TransformDirection(Vector3.forward) * 50.0F, Color.red);
               
                //if (hit.transform.gameObject.tag == "Player")
               // {
                    if (firing == false)
                    {
                        firing = true;
                        fireTime = Time.time;
                        navMeshAgent.speed = 0.0f;
                        animator.SetInteger("State", 4);
                        Debug.Log("Fire");
                  
                  //  animator.GetCurrentAnimatorStateInfo(0).IsName("rig|ThrowEnd");
                       //StartCoroutine(makingSnowBall());

                        //animator.SetInteger("State", 3);

                       // GameObject tempSnowball = GameObject.Instantiate(bullet, spawn.transform.position, transform.rotation);

                        // This bit is added by Ekin. Because my snowballs don't have any scripts attached to them, I just add force 
                        // to their rigid bodies.
                        /*if (bullet.tag == "Snowball")
                        {
                            Vector3 direction = player.transform.position - spawn.transform.position;
                            Vector3.Normalize(direction);
                            tempSnowball.GetComponent<Rigidbody>().useGravity = false;
                            tempSnowball.GetComponent<Rigidbody>().AddForce(direction * snowballSpeed);
                            tempSnowball.GetComponent<Snowball>().SetTag("Enemy Snowball");
                        } // End of Ekin's bit.*/
                    }
                   StartCoroutine(shootingDelay());
               // }
           // }
        }

        if (firing && fireTime + coolDown <= Time.time) firing = false;
    }

    bool PlayerInRange()
    {
        return (Vector3.Distance(player.transform.position, transform.position) <= rangeToPlayer);
    }

    IEnumerator shootingDelay()
    {
        yield return new WaitForSeconds(3.0f);
        navMeshAgent.speed = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Snowball")
        {
            Debug.Log("Hit Detected");
            //Destroy(this.gameObject);
            navMeshAgent.speed = 0;
            animator.SetInteger("State", 5);
            StartCoroutine(hitDelay());
            SpawnWaves.numOfEnemies--; 
        };

    }

    IEnumerator hitDelay()
    {
        yield return new WaitForSeconds(1.0f);
        navMeshAgent.speed = speed;
        animator.SetInteger("State", 1);
    }


    public void SpawnBullet()
    {
        Instantiate(bullet, spawn.transform.position, spawn.transform.rotation);
       Debug.Log(spawn.transform.position);
    }


    private void lookAtPlayer()
    {
        Vector3 difference = player.transform.position - transform.position;
        //float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //float rotX = Mathf.Atan2(difference.z, difference.y) * Mathf.Rad2Deg;
        float rotY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);
    }

    public bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {

        // if (overlaps[i] != null)
        // {
        if (PlayerInRange())
        {
            // if (overlaps[i].gameObject.tag == "Player")
            //  {

            Vector3 directionBetween = (target.position - checkingObject.position).normalized;
            directionBetween.y *= 0;

            float angle = Vector3.Angle(checkingObject.forward, directionBetween);

            if (angle <= maxAngle)
            {

                Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxRadius))
                {
                    //Debug.DrawRay(checkingObject.position, checkingObject.forward * 50.0F, Color.red);
                    if (hit.transform.tag == "Player")

                        return true;

                }


                // }
            }
        }
        return false;
    }
        
          //  }

           /* Collider[] overlaps = new Collider[100];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {

                if (overlaps[i].gameObject.tag == "Player")
                {

                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {

                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            //Debug.DrawRay(checkingObject.position, checkingObject.forward * 50.0F, Color.red);
                            if (hit.transform.tag == "Player")
                                
                                return true;

                        }


                    }


                }

            }

        }

        return false;
    }*/

}
