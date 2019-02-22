using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    private int size;
    private bool isColliding;
    public float sizeIncreaseAmount;

    void Start()
    {
        size = 1;
        isColliding = false;
    }
    
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Snowball" && !isColliding)
        {
            isColliding = true;
            collision.gameObject.GetComponent<Snowball>().SetColliding(true);
            if (collision.transform.position.y > transform.position.y)
            {
                IncreaseSize(collision.gameObject.GetComponent<Snowball>().GetSize());
                Destroy(collision.gameObject, 0.001f);
            }
            else
            {
                collision.gameObject.GetComponent<Snowball>().IncreaseSize(size);
                Destroy(gameObject, 0.001f);
            }
        }

        if (collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Snowball")
        {
            isColliding = false;
        }
    }

    public void IncreaseSize(int multiplier)
    {
        transform.localScale += new Vector3(sizeIncreaseAmount, sizeIncreaseAmount, sizeIncreaseAmount) * multiplier;
        size++;
    }

    public int GetSize()
    {
        return size;
    }

    public void SetColliding(bool flag)
    {
        isColliding = flag;
    }
}
