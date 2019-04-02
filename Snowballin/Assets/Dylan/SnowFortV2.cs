using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFortV2 : MonoBehaviour
{
    public float snowSize = 1f;
    SnowFortV2 otherStackV2;
    public bool top, bottom;

    public float delay = 0.2f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy Snowball"))
        {
            Decrease(0.5f);
            return;
        }

        Invoke("Kinematicize", delay);

        if (other.CompareTag("FortBall"))
        {
            if (other.gameObject.transform.position.y > transform.position.y && other.gameObject.transform.position.y > 17)
            {
                otherStackV2 = other.GetComponent<SnowFortV2>();
                otherStackV2.top = true;
                bottom = true;
                top = false;
            }
        }
    }

    private void Update()
    {
        transform.localScale = Vector3.one * snowSize;
        if (snowSize <= 1.5)
            Die();
           
    }

    public void Increase()
    {
        if (!bottom && !top)
        {
            snowSize += 0.2f;
            transform.Translate(0f, 0.1f, 0f);
            transform.Rotate(0f, 0f, -30f);
        }
    }

    public void Decrease(float snowStrength)
    {
        if (bottom)
        {
            if(otherStackV2.snowSize - snowStrength < 2)
            {
                otherStackV2.Decrease(snowStrength);
                otherStackV2 = null;
                bottom = false;
            }
            else
            otherStackV2.Decrease(snowStrength);
        }
        else
        {
            snowSize -= snowStrength;
            transform.Translate(0f, -snowStrength / 0.5f, 0f, Space.World);
        }
    }

    public void Kinematicize()
    {
        rb.isKinematic = true;
    }

   public void DeKinematic()
    {
        rb.isKinematic = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
