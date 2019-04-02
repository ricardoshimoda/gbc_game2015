using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public Material originalMaterial;
    public Material shinyMaterial;
    public Renderer button;
    public float duration = 0.5f;

    public bool blinking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(blinking){
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            button.material.Lerp(originalMaterial, shinyMaterial, lerp);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(blinking){
            Destroy(this.gameObject, 3);
        }
    }

}
