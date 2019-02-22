using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SnowPile : MonoBehaviour
{
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject snowballPrefab;
    public GameObject snowHeight;

    public bool pointerTrigger = false;

    private void Update()
    {
        RemoveYConstraintFromSnowball();
        if (leftHandAnchor.transform.position.y < snowHeight.transform.position.y)
        {
            if(pointerTrigger){
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Instantiate(snowballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                    SteamVR_Actions.default_Haptic.Execute(0,0.5f,20,1,SteamVR_Input_Sources.LeftHand);
                }
            }
            else{
                if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Instantiate(snowballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                    SteamVR_Actions.default_Haptic.Execute(0,0.5f,20,1,SteamVR_Input_Sources.LeftHand);
                }
            }
        }
        if (rightHandAnchor.transform.position.y < snowHeight.transform.position.y)
        {
            if(pointerTrigger){
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Instantiate(snowballPrefab, leftHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                    SteamVR_Actions.default_Haptic.Execute(0,0.5f,20,1,SteamVR_Input_Sources.RightHand);
                }
            }
            else{
                if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Instantiate(snowballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                    SteamVR_Actions.default_Haptic.Execute(0,0.5f,20,1,SteamVR_Input_Sources.RightHand);
                }
            }
        }
    }

    void RemoveYConstraintFromSnowball()
    {
        GameObject[] snowballs = GameObject.FindGameObjectsWithTag("Snowball");
        for (int i = 0; i < snowballs.Length; i++)
        {
            if (snowballs[i].GetComponent<Throwable>().GetAttached())
            {
                snowballs[i].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }
    }
}
