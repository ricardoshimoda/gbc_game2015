using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiPrey : MonoBehaviour
{
    public bool prey = true;

    public bool hiding = true;
    public float hidingTimer, hidingTime = 3f;

    void Update()
    {
        if (prey)
        {
            if (hiding == true)
            {
                hidingTimer += Time.deltaTime;
                //transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if (hidingTimer >= hidingTime)
            {
                hiding = !hiding;
                hidingTimer = 0f;
            }
            if (hiding == false)
            {
                hidingTimer += Time.deltaTime;
                //transform.localScale = new Vector3(1f, 3f, 1f);
            }
        }
        if(prey)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        if (!prey)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
