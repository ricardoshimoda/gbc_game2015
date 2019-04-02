using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Animator Chidler;
    private void Start()
    {
        Chidler = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            Chidler.SetInteger("State", 0);
        }
        if (Input.GetKeyDown("1"))
        {
            Chidler.SetInteger("State", 1);
        }
        if (Input.GetKeyDown("2"))
        {
            Chidler.SetInteger("State", 2);
        }
        if (Input.GetKeyDown("3"))
        {
            Chidler.SetInteger("State", 3);
        }
        if (Input.GetKeyDown("4"))
        {
            Chidler.SetInteger("State", 4);
        }
        if (Input.GetKeyDown("5"))
        {
            Chidler.SetInteger("State", 5);
        }
        if (Input.GetKeyDown("6"))
        {
            Chidler.SetInteger("State", -1);
        }
    }
}
