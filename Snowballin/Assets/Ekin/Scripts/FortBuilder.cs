using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FortBuilder : MonoBehaviour
{
    public GameObject fortPrefab;
    public int raycastCountHorizontal;                  // The total number of raycasts in the horizontal axis is the central raycast + 2 * raycastCountHorizontal. 
    public int raycastCountVertical;                    // The total number of raycasts in the horizontal axis is the central raycast + 2 * raycastCountVertical. 
    public float raycastGap;                            // The length of the gap between each raycast.
    public float fortBuildSpeed;                        // The build speed of the snow forts.
    public float maxFortHeight;                         // The maximum height of the fort.
    public AudioClip[] audioClips;

    private GameObject leftHandAnchor, rightHandAnchor; // Represents the position of the controllers.
    private float snowHeight;                           // Distance between the ground and the snow pile.
    private GameObject debugText;                       // Used for debugging while wearing the headset. It is 3D Text attached to the head view.
    private float yOffset;
    private AudioSource sfxAudioSource;
    private RaycastHit hit;                                     

    void Start()
    {
        sfxAudioSource = GameObject.Find("Audio SFX").GetComponent<AudioSource>();
        yOffset = -0.55f;
        raycastCountHorizontal = 1;
        raycastCountVertical = 1;
        rightHandAnchor = GameObject.FindGameObjectWithTag("RightHandCollider");
        leftHandAnchor = GameObject.FindGameObjectWithTag("LeftHandCollider");
        snowHeight = GameObject.FindGameObjectWithTag("Snow Pile").transform.position.y;
        debugText = GameObject.FindGameObjectWithTag("Debug Text");
    }
    
    void Update()
    {
        MakeSnowFort();
    }

    void MakeSnowFort()
    {
        if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.RightHand) && GameObject.FindGameObjectWithTag("Attached Snowball") != null)
        { // Right Hand
            CastRay(SteamVR_Input_Sources.RightHand, rightHandAnchor.transform.position, fortBuildSpeed); // Central Raycast.
            for (int i = 0; i < raycastCountHorizontal; i++)
            { // Horizontal raycasts.
                Vector3 raycastOffset = new Vector3((i + 1) * raycastGap, 0, 0);
                CastRay(SteamVR_Input_Sources.RightHand, rightHandAnchor.transform.position + raycastOffset, fortBuildSpeed);
                CastRay(SteamVR_Input_Sources.RightHand, rightHandAnchor.transform.position - raycastOffset, fortBuildSpeed);
            }
            for (int i = 0; i < raycastCountVertical; i++)
            { // Vertical raycasts.
                Vector3 raycastOffset = new Vector3(0, 0, (i + 1) * raycastGap);
                CastRay(SteamVR_Input_Sources.RightHand, rightHandAnchor.transform.position + raycastOffset, fortBuildSpeed);
                CastRay(SteamVR_Input_Sources.RightHand, rightHandAnchor.transform.position - raycastOffset, fortBuildSpeed);
            }
        }
        else if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.LeftHand) && GameObject.FindGameObjectWithTag("Attached Snowball") != null)
        { // Left Hand
            CastRay(SteamVR_Input_Sources.LeftHand, leftHandAnchor.transform.position, fortBuildSpeed); // Central Raycast.
            for (int i = 0; i < raycastCountHorizontal; i++)
            { // Horizontal raycasts.
                Vector3 raycastOffset = new Vector3((i + 1) * raycastGap, 0, 0);
                CastRay(SteamVR_Input_Sources.LeftHand, leftHandAnchor.transform.position + raycastOffset, fortBuildSpeed);
                CastRay(SteamVR_Input_Sources.LeftHand, leftHandAnchor.transform.position - raycastOffset, fortBuildSpeed);
            }                                                                                                             
            for (int i = 0; i < raycastCountVertical; i++)                                                                
            { // Vertical raycasts.                                                                                       
                Vector3 raycastOffset = new Vector3(0, 0, (i + 1) * raycastGap);                                          
                CastRay(SteamVR_Input_Sources.LeftHand, leftHandAnchor.transform.position + raycastOffset, fortBuildSpeed);
                CastRay(SteamVR_Input_Sources.LeftHand, leftHandAnchor.transform.position - raycastOffset, fortBuildSpeed);
            }
        }
    }

    void CastRay(SteamVR_Input_Sources hand, Vector3 position, float raycastStrength)
    {
        GameObject currentHandAnchor;
        if (hand == SteamVR_Input_Sources.RightHand)
        {
            currentHandAnchor = rightHandAnchor;
        }
        else
        {
            currentHandAnchor = leftHandAnchor;
        }
        if (Physics.Raycast(position, -Vector3.up, out hit))
        {
            GameObject attachedSnowball = GameObject.FindGameObjectWithTag("Attached Snowball");
            if (attachedSnowball != null && !attachedSnowball.GetComponent<Snowball>().Consumed())
            {
                if (hit.transform.gameObject.tag == "Floor")
                {
                    Vector3 spawnPosition = new Vector3(position.x, position.y - hit.distance + snowHeight + yOffset, position.z);
                    GameObject tempFort = Instantiate(fortPrefab, spawnPosition, Quaternion.identity);
                    tempFort.GetComponent<SnowFort>().SetStopLocation(position.y - hit.distance + snowHeight + yOffset + maxFortHeight);
                }
                else if (hit.transform.gameObject.tag == "Fort")
                {
                    if (!sfxAudioSource.isPlaying)
                    {
                        sfxAudioSource.clip = audioClips[0];
                        sfxAudioSource.Play();
                    }
                    hit.transform.gameObject.GetComponent<SnowFort>().AdjustHeight(raycastStrength);
                }
            }
        }
    }

    public float GetRaycastGap()
    {
        return raycastGap;
    }
}
