using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void FlipOver()
    {
        anim.SetBool("Flip", true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
