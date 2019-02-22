using UnityEngine;
using System.Collections;
using System.Linq;

public class AI : MonoBehaviour {
	public Camera sourceCam;
	public GameObject targetObj;
		
	public float slowDistPerc; // Percentage of distance away from target location to toggle slow down.
	private float slowDist;
	public float stopDist;
	public float slowRotPerc; // Percentage of rotation away from target rotation to toggle slow down.
	private float slowRot;
	private float rotLeft; // Rotation remaining to destination rotation.
	
	private float velocity = 0.0F; // Linear velocity.
	private float rotation = 0.0F; // Angular velocity.
	public float velocityMax;
	public float rotationMax;
	private float accelLinear;
	private float accelAngular;
	public float accelLinearInc;
	public float accelAngularInc;
	public float accelLinearMax;
	public float accelAngularMax;
		
	private bool hasTarget = false;
	
	private Vector3 moveTarget;
	private Vector3 destVect;
	private Quaternion destRot;
	private float distTo;
	
	//private int wpIndex = 0;
	public GameObject[] waypoints;
	public bool stopAtLastWP;

    public float rangeToPlayer;
    public GameObject bullet;
    public GameObject spawn;
    private GameObject player;
    private bool firing = false;
    private float fireTime;
    private float coolDown = 1.0F;
    private bool isHiding = false;
    private bool isSlotEmpty = true;
	
	void Start () {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToArray(); //.OrderBy(go => go.name)
        player = GameObject.FindWithTag("Player");
        moveTarget = waypoints[ Random.Range(0, waypoints.Length) ].transform.position;
        targetObj.transform.position = moveTarget;
       // waypoints[Random.Range(0, waypoints.Length)].transform.position = moveTarget;
		hasTarget = true;
		StartMoving();
	}
	
	void Update () {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.gameObject.tag == "Enemies" || hit.transform.gameObject.tag == "Trees")
            {
                //isSlotEmpty = false;
                moveTarget = waypoints[Random.Range(0, waypoints.Length)].transform.position;
                targetObj.transform.position = moveTarget;
            }
           // else isSlotEmpty = true;
        }
        
        if ( hasTarget) {
           
			// Set the destination vector and rotation.
			destVect = moveTarget - transform.position;
			distTo = destVect.magnitude; // Vector3.Distance(transform.position, moveTarget) does the same thing.

			destRot = Quaternion.LookRotation( destVect );
			rotLeft = Quaternion.Angle(transform.rotation, destRot);
			
			transform.Translate( Vector3.forward * GetMoveSpeed() * Time.deltaTime );
			velocity = Mathf.Clamp(velocity + accelLinear, 0.0F, velocityMax);
			accelLinear = Mathf.Clamp(accelLinear + accelLinearInc, 0.0F, accelLinearMax);
					
			transform.rotation = Quaternion.RotateTowards( transform.rotation, destRot, GetRotSpeed() * Time.deltaTime );
			rotation = Mathf.Clamp((rotation + accelAngular), 0.0F, rotationMax);
			accelAngular = Mathf.Clamp((accelAngular + accelAngularInc), 0.0F, accelAngularMax);
									
			transform.eulerAngles = new Vector3( 0.0F, transform.eulerAngles.y, 0.0F ); // Ensure no rotation on X-Z axes.

            int percent = Random.Range(0, 3); //50%
            if(percent == 0)
            {
                Debug.Log("Shoot");
                Shooting();
            }
            if (distTo < stopDist) {
				//wpIndex++;
				//if ( wpIndex == waypoints.Length )	wpIndex = 0;
                moveTarget = waypoints[ Random.Range(0, waypoints.Length) ].transform.position;
				targetObj.transform.position = moveTarget;
                velocity = 0.0F;
                Debug.Log("Reach");
                isHiding = true;
                hasTarget = false;
                Shooting();
                StartCoroutine(Hide());
				StartMoving();
			}	
		}
       
	}
	
	void StartMoving()
	{
		accelLinear = 0.0F;
		accelAngular = 0.0F;
		rotation = 0.0F;
		
		destVect = moveTarget - transform.position;
		distTo = destVect.magnitude; // Vector3.Distance(transform.position, moveTarget) does the same thing.
		slowDist = distTo * slowDistPerc;
		
		destRot = Quaternion.LookRotation( destVect );
		rotLeft = Quaternion.Angle(transform.rotation, destRot);
		slowRot = rotLeft * slowRotPerc;
	}
	
	float GetMoveSpeed()
	{
		return ((stopAtLastWP && distTo >= slowDist)?Mathf.Lerp(0.0F, velocity, distTo/slowDist):velocity);
	}
	
	float GetRotSpeed()
	{
		return (rotLeft >= slowRot?rotation:Mathf.Lerp(0.0F, rotation, rotLeft/slowRot));
	}

    IEnumerator Hide()
    {
        int delayTime = Random.Range(0, 5);
        //int delayTime = 10;
        yield return new WaitForSeconds(delayTime);
        hasTarget = true;
        isHiding = false;
    }

    IEnumerator shootingDelay()
    {
        yield return new WaitForSeconds(1);
        hasTarget = true;
    }

   


    void Shooting()
    {
        if (PlayerInRange())
        {
            //Debug.Log("Target");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 20.0F, Color.red);
                Debug.Log("ASDF");
                if (hit.transform.gameObject.tag == "Player")
                {

                    if (firing == false)
                    {
                        firing = true;
                        fireTime = Time.time;
                        GameObject.Instantiate(bullet, spawn.transform.position, transform.rotation);
                        if(!isHiding)
                        {
                            hasTarget = false;
                            StartCoroutine(shootingDelay());  
                        }
                       
                    }

                }
            }
        }

        if (firing && fireTime + coolDown <= Time.time) firing = false;
    }

    bool PlayerInRange()
    {
        return (Vector3.Distance(player.transform.position, transform.position) <= rangeToPlayer);
    }
    
    // NOTE: Snowball script takes care of destroying its target. No need to call Destroy() here.
    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Snowball")
        {
            Destroy(this.gameObject);
        }
    }*/
   
}
