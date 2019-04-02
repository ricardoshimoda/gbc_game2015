using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStack : MonoBehaviour
{
    public float delay = 0.2f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Invoke("Kinematicize", delay);
    }

    void Kinematicize()
    {
        rb.isKinematic = true;
    }
}
