using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiRoar : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roar(){
        aud.Play();
    }

}
