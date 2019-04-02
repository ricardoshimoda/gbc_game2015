using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnContact : MonoBehaviour
{
    string killer = "Terrain";
    public GameObject effect, effect2;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EndLife();
        }
        if (other.CompareTag("FortBall"))
        {
            EndLife();
        }
    }


    private void EndLife()
    {
        Instantiate(effect, transform.position, transform.rotation);
        Instantiate(effect2, transform.position, transform.rotation);
        Destroy(effect, 1f);
        Destroy(effect2, 1f);
        Destroy(this.gameObject);
    }
}
