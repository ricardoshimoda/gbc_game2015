using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Fort : MonoBehaviour
{
    public float scaleUpFrequency;
    public float scaleUpMagnitude;
    public float startMeltingAfterSeconds;
    public float meltFrequency;
    public float meltMagnitude;

    private GameObject debugText;       // Used for debugging while wearing the headset. It is 3D Text attached to the head view.
    private bool leftHandColliding;
    private bool rightHandColliding;   
    private bool isScalingUp;           // Used to prevent InvokeRepeating() from getting called multiple times on ScaleUpFort().
    
    void Start()
    {
        debugText = GameObject.FindGameObjectWithTag("Debug Text");
        leftHandColliding = false;
        rightHandColliding = false;
        InvokeRepeating("MeltFort", startMeltingAfterSeconds, meltFrequency);
    }
    
    void Update()
    {
        ManageFortSize();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LeftHandCollider")
        {
            leftHandColliding = true;
        }
        if (other.gameObject.tag == "RightHandCollider")
        {
            rightHandColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LeftHandCollider")
        {
            leftHandColliding = false;
        }
        if (other.gameObject.tag == "RightHandCollider")
        {
            rightHandColliding = false;
        }
    }

    private void ManageFortSize()
    {
        if (IsPressingGrabPinch() && !isScalingUp)
        {
            debugText.GetComponent<TextMesh>().text += "HERE\n";
            InvokeRepeating("ScaleUpFort", 0, scaleUpFrequency);
            isScalingUp = true;
        }
        else if (!IsPressingGrabPinch())
        {
            CancelInvoke("ScaleUpFort");
            isScalingUp = false; ;
        }
    }

    private void ScaleUpFort()
    {
        Vector3 tempScale = transform.localScale;
        tempScale.y += scaleUpMagnitude;
        transform.localScale = tempScale;
    }

    private bool IsPressingGrabPinch()
    {
        if (leftHandColliding && rightHandColliding)
        {
            if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.RightHand) && SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.LeftHand))
            {
                //debugText.GetComponent<TextMesh>().text = "True\n";
                return true;
            }
        }
        //debugText.GetComponent<TextMesh>().text = "False\n";
        return false;
    }

    private void MeltFort()
    {
        Vector3 tempScale = transform.localScale;
        tempScale.y -= meltMagnitude;
        transform.localScale = tempScale;
        if (transform.localScale.y < 1.4)
        {
            CancelInvoke("MeltFort");
            Destroy(gameObject);
        }
    }
}
