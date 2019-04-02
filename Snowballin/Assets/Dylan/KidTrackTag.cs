using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidTrackTag : MonoBehaviour
{
    KidTracker kt;

    // Start is called before the first frame update
    void Start()
    {
        kt = KidTracker.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!kt.kidsAlive.Contains(this.gameObject))
        {
            kt.kidsAlive.Add(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        kt.kidsAlive.Remove(this.gameObject);
    }
}
