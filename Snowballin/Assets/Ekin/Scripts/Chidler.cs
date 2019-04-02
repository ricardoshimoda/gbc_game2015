using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chidler : MonoBehaviour
{
    public float waitTimeAtWaypoint;
    public float snowballShootFrequency;
    public float peekFrequency;
    public float snowballSpeed;
    public GameObject snowballPrefab;
    public GameObject snowballSpawnPoint;
    public float maxHeight = 3.0f;
    public float gravity = -2.0f;

    private int chidlerId;
    private Transform retreatPosition;
    private GameObject[] waypoints;
    private GameObject player;
    private GameObject currentWaypoint;
    private bool isMoving, isDestroyed;
    private Animator animator;
    private AudioSource audioSource;
    private AudioManager audioManager;
    private NavMeshAgent navAgent;
  
    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        navAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        retreatPosition = GameObject.FindGameObjectWithTag("Spawn Point").transform;
        SelectWaypoint();
        InvokeRepeating("Trip", 2, 4);
      
    }

    // Update is called once per frame
    void Update()
    {
        MoveToWaypoint();
        UpdateMoveAnimation();
        Destroy();
    }

    List<GameObject> GetFreeWaypoints()
    {
        List<GameObject> freeWaypoints = new List<GameObject>();
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (!waypoints[i].GetComponent<Waypoint>().GetOccupied())
            {
                freeWaypoints.Add(waypoints[i]);
            }
        }
        return freeWaypoints;
    }

    void Trip()
    {
        if (isMoving)
        {
            int roll = Random.Range(1, 101);
            if (roll < 33)
            {
                audioSource.clip = audioManager.GetOuch(chidlerId);
                audioSource.Play();
                animator.SetBool("isTripping", true);
            }
        }
    }

    void EndTrip()
    {
        animator.SetBool("isTripping", false);
    }

    void SelectWaypoint()
    {
        if (!animator.GetBool("isRetreating") && !animator.GetBool("isHit"))
        {
            List<GameObject> freeWaypoints = GetFreeWaypoints();
            GameObject closestWaypoint = freeWaypoints[0];
            for (int i = 1; i < freeWaypoints.Count; i++)
            {
                if (Vector3.Distance(transform.position, freeWaypoints[i].transform.position) <
                    Vector3.Distance(transform.position, closestWaypoint.transform.position))
                {
                    closestWaypoint = freeWaypoints[i];
                }
            }
            GameObject nextWaypoint = closestWaypoint;
            if (currentWaypoint != null)
            {
                currentWaypoint.GetComponent<Waypoint>().SetOccupied(false);
            }
            nextWaypoint.GetComponent<Waypoint>().SetOccupied(true);
            isMoving = true;
            transform.LookAt(nextWaypoint.transform);
            currentWaypoint = nextWaypoint;
            CancelInvoke("StartShootAnimation");
        }
    }

    void MoveToWaypoint()
    {
        if (animator.GetBool("isHit") || animator.GetBool("isTripping"))
        {
            navAgent.isStopped = true;
        }
        else if (animator.GetBool("isRetreating"))
        {
            navAgent.isStopped = false;
            navAgent.destination = retreatPosition.position;
        }
        else if (isMoving && currentWaypoint != null)
        {
            navAgent.isStopped = false;
            navAgent.destination = currentWaypoint.transform.position;
            Vector2 chidlerPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetPosition = new Vector2(currentWaypoint.transform.position.x, currentWaypoint.transform.position.z);
            if (chidlerPosition == targetPosition)
            {
                transform.LookAt(player.transform);
                isMoving = false;
                Invoke("SelectWaypoint", waitTimeAtWaypoint);
                if (currentWaypoint.GetComponent<Waypoint>().behindCover)
                {
                    InvokeRepeating("StartPeekAnimation", 0, peekFrequency);
                }
                else
                {
                    InvokeRepeating("StartShootAnimation", 0, snowballShootFrequency);
                }
            }
        }
        else
        {
            UpdateRotation();
        }
    }

    void ShootSnowball()
    {
        RaycastHit hit;
        Vector3 direction = player.transform.position - snowballSpawnPoint.transform.position;
        direction.Normalize();
        if (Physics.Raycast(snowballSpawnPoint.transform.position, direction, out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                //GameObject testObejct = GameObject.FindWithTag("TestObject");
                GameObject tempSnowball = Instantiate(snowballPrefab, snowballSpawnPoint.transform.position, snowballSpawnPoint.transform.rotation);
                tempSnowball.GetComponent<Rigidbody>().useGravity = false;
                tempSnowball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                tempSnowball.GetComponent<Rigidbody>().mass = 1.0f;
                Physics.gravity = Vector3.up * gravity;
                tempSnowball.GetComponent<Rigidbody>().useGravity = true;
                tempSnowball.GetComponent<Rigidbody>().velocity = caculateVelocity(player.transform, snowballSpawnPoint.transform);
          
           
                tempSnowball.GetComponent<Snowball>().SetTag("Enemy Snowball");
            }
        }
        //animator.SetBool("isShooting", false);
    }

    void UpdateRotation()
    {
        transform.LookAt(player.transform);
        Vector3 tempAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.eulerAngles = tempAngles;
    }

    void StartShootAnimation()
    {
        animator.SetBool("isShooting", true);
    }

    void EndShootAnimation()
    {
        animator.SetBool("isShooting", false);
    }

    void StartPeekAnimation()
    {
        animator.SetBool("isPeeking", true);
    }

    void EndPeekAnimation()
    {
        animator.SetBool("isPeeking", false);
    }

    void StartDeathAnimation()
    {
        audioSource.clip = audioManager.GetOuch(chidlerId);
        audioSource.Play();
        animator.SetBool("playingDeathAnimation", true);
    }

    void StartRetreating()
    {
        audioSource.clip = audioManager.GetCry(chidlerId);
        audioSource.Play();
        animator.SetBool("isHit", false);
        animator.SetBool("isRetreating", true);
        currentWaypoint.GetComponent<Waypoint>().SetOccupied(false);
    }

    void Destroy()
    {
        Vector2 chidlerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPosition = new Vector2(retreatPosition.position.x, retreatPosition.position.z);
        if (animator.GetBool("isRetreating") && chidlerPosition == targetPosition && !isDestroyed)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    void UpdateMoveAnimation()
    {
        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void PlayFootstep()
    {
        audioSource.clip = audioManager.GetSnowStep();
        audioSource.Play();
    }

    void PlayThrowSound()
    {
        Debug.Log("ID " + chidlerId);
        audioSource.clip = audioManager.GetSnowballThrowEnemy(chidlerId);
        audioSource.Play();
    }

    public void SetId(int id)
    {
        chidlerId = id;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snowball")
        {
            Debug.Log("HASDHASDHASDH");
            animator.SetBool("isHit", true);
        }
    }

    Vector3 caculateVelocity(Transform target, Transform spawnPoint)
    {
        //V^2 - V0^2 = 2 * a * s => going up, at the highest point V = 0, a = g => V0 = sqrt(V^2 - 2as) = Sqr(-2 * g * maxHeight)
        //V = V0 + at => time going up T1 = -V0/a = -V0/g = -Sqrt(-2 * g * maxHeight)/g = -Sqrt((-2 * maxHeight)/g)
        // s = v0 * t + 0.5 * a * t^2 => maxHeight - initialHeight = 0 * t + 0.5 * g * t^ 2 => time going down T2 = Sqrt((maxHeight - initialHeight)/ 0.5g)
        // => t = T1 + T2
        // distance = Vx * t + 0.5at^2 => no friction a = 0 => Vx = distance / t
        // V = Vx+ Vy

        // **** Done with Math, Let code it ****



        float initialHeight = target.position.y - spawnPoint.position.y;
        Vector3 distance = new Vector3(target.position.x - spawnPoint.position.x, 0, target.position.z - spawnPoint.position.z);
        Vector3 Vy = Vector3.up * Mathf.Sqrt(Mathf.Abs(-2 * gravity * maxHeight));
        float timeUp = Mathf.Sqrt(Mathf.Abs(-2 * maxHeight / gravity));
        float timeDown = Mathf.Sqrt(Mathf.Abs(2 * (-initialHeight + maxHeight) / gravity));
        float time = timeUp + timeDown;
        Vector3 Vx = distance / time;
        return Vx + Vy ;
       
        //I have no idea why it sitll aims to the back of the player @_@ :( :(

    }
}
