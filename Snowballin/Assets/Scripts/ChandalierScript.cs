using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandalierScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        anim.SetBool("Drop", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Table"))
        {
            TableScript ts = other.GetComponent<TableScript>();
            ts.FlipOver();
        }
    }
}
