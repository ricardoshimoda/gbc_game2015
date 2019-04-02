using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flycam : MonoBehaviour
{
    public float interval = 10.0f;
    public float speed = 30.0f;

    public int minChidlers = 100;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
      // float angle = 
        var chidlers = GameObject.FindGameObjectsWithTag("Target");
        if ((chidlers.Length > minChidlers)){
            if(transform.eulerAngles.x > 30.0f)
            {
                transform.RotateAround(Vector3.zero, Vector3.right, interval * Time.deltaTime);
               // Debug.Log(transform.eulerAngles.x);   
            } 
        // if(transform.eulerAngles.x < 30.0f)
            //    transform.RotateAround(Vector3.zero, Vector3.left, interval * Time.deltaTime);
                
        transform.LookAt(Vector3.zero);
        // GameObject.GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>.fieldOfView(), zoom, Time.deltaTime * smooth);
            StartCoroutine(Timer());

        }

    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(7.0f);
         transform.Translate(Vector3.forward * speed * Time.deltaTime);

        StartCoroutine(Timer2());
    }

    IEnumerator Timer2()
    {
        yield return new WaitForSeconds(5.0f);
        speed = 0.0f;

    }
}
