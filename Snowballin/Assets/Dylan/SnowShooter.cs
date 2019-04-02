using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowShooter : MonoBehaviour
{
    public GameObject snowball;
    public float snowforce;
    public bool shoot;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleShoot()
    {
        shoot = !shoot;
    }

    IEnumerator Shoot()
    {
        while (transform)
        {
            if (shoot == true)
            {
                GameObject snowball1 = Instantiate(snowball, transform.position, transform.rotation);
                Rigidbody rb = snowball1.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * snowforce);
                Destroy(snowball1, 5f);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
