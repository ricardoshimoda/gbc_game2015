using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballPuff : MonoBehaviour
{
    public GameObject snowballPuffPrefab;

    public void StartPuffAnimation(Transform transform)
    {
        GameObject tempPuff = Instantiate(snowballPuffPrefab, transform.position, transform.rotation);
        float destroyAfter = tempPuff.GetComponent<ParticleSystem>().main.duration;
        Destroy(tempPuff, destroyAfter);
    }
}
