using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFort : MonoBehaviour
{
    private float stopLocationY;                        // The y coordinate of the stop position of this fort.
    private GameObject snowPile;                        // The snow pile prefab. used to access FortBuilder script.
    private GameObject leftHandAnchor, rightHandAnchor; // Represents the position of the controllers.
    private bool isLocked;                              // Locked forts reached their maximum height. They will not move.

    void Start()
    {
        snowPile = GameObject.FindGameObjectWithTag("Snow Pile");
        isLocked = false;
        AdjustScale();
    }
    
    void Update()
    {
        
    }

    void AdjustScale()
    {
        float raycastGap = snowPile.GetComponent<FortBuilder>().GetRaycastGap();
        transform.localScale = new Vector3(raycastGap, transform.localScale.y, raycastGap);
    }

    public void AdjustHeight(float amount)
    {
        if (!isLocked)
        {
            GameObject attachedSnowball = GameObject.FindGameObjectWithTag("Attached Snowball");
            if (attachedSnowball != null && !attachedSnowball.GetComponent<Snowball>().Consumed())
            {
                transform.Translate(0, amount, 0);
                if (transform.position.y >= stopLocationY)
                {
                    isLocked = true;
                }
                attachedSnowball.GetComponent<Snowball>().ConsumeSnowball();
            }
        }
    }

    public void SetStopLocation(float location)
    {
        stopLocationY = location;
    }
}
