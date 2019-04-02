using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public GameObject target;
    public float speed;

    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.Translate(new Vector3(-1 * speed * Time.deltaTime, 0, 0));
    }
}
