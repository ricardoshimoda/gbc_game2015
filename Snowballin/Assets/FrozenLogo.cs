using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLogo : MonoBehaviour
{
    MeshRenderer rend;
    float perc = 0.0f;
    float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        Debug.Log(rend.material.shader);
        rend.material.SetFloat("_Ice_fresnel",0);
    }

    // Update is called once per frame
    void Update()
    {
        perc += speed * Time.deltaTime;
             Debug.Log("uepa" + perc);
        if(perc < 85){
            rend.material.SetFloat("_Ice_fresnel",(1-perc/100)*3);
        }
        else if(perc > 90){
            perc = 0;
            rend.enabled = true;
        }
        else{
            rend.enabled = false;
        }

    }
}
