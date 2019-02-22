using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	public float rangeToPlayer;
	public GameObject bullet;
    public GameObject spawn;
	private GameObject player;
	private bool firing = false;
	private float fireTime;
	private float coolDown = 1.0F;
	
	void Start () 
    {
		player = GameObject.FindWithTag("Player");
       // transform.rotation = Quaternion.identity;
	}
	
	void Update () 
    {
       
        //transform.LookAt(player.transform.position);
		if ( PlayerInRange() ) 
        {
          //  Debug.Log("Target");
			RaycastHit hit;

			if( Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit )) 
            {
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 20.0F, Color.red);


				if (hit.transform.gameObject.tag == "Player") 
                {
                    
					if ( firing == false )
                    {     
						firing = true;
                        fireTime = Time.time;
						GameObject.Instantiate(bullet, spawn.transform.position, transform.rotation);
					}

				}
			}
		}

		if ( firing && fireTime + coolDown <= Time.time )  firing = false;
	}
	

	bool PlayerInRange() 
    {
		return ( Vector3.Distance(player.transform.position, transform.position) <= rangeToPlayer );
	}
}
