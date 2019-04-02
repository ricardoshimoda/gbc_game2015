using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SnowPile : MonoBehaviour
{
    public AudioClip[] audioClips;
    public GameObject snowballPrefab;
    public GameObject playerHead;                       // Used to determine the rotation of the snow fort.

    private GameObject leftHandAnchor, rightHandAnchor; // Represents the position of the controllers.
    private float snowHeight;                           // Distance between the ground and the snow pile.
    private GameObject debugText;                       // Used for debugging while wearing the headset. It is 3D Text attached to the head view.
    private GameObject scoreText;                       // 3D Text that shows player score.
    private int playerScore;
    private GameObject snowballContainer;
    private AudioSource sfxAudioSource;
    private AudioManager audioManager;

    private void Start()
    {
        rightHandAnchor = GameObject.FindGameObjectWithTag("RightHandCollider");
        leftHandAnchor = GameObject.FindGameObjectWithTag("LeftHandCollider");
        snowHeight = GameObject.FindGameObjectWithTag("Snow Pile").transform.position.y;
        debugText = GameObject.FindGameObjectWithTag("Debug Text");
        scoreText = GameObject.FindGameObjectWithTag("Score Text");
        snowballContainer = GameObject.FindGameObjectWithTag("Snowball Container");
        playerScore = 0;
        sfxAudioSource = GameObject.Find("Audio SFX").GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public bool pointerTrigger = false;

    private void Update()
    {
        AddGravityToSnowball();
        UpdateSnowballTags();
        MakeSnowballFlat();
    }

    // This function sets the gravity property of the rigid body component of the snowballs attached to either hand to true.
    // This alllows the player to catch enemy snowballs and throw them back properly.
    void AddGravityToSnowball()
    {
        GameObject[] snowballs = GameObject.FindGameObjectsWithTag("Attached Snowball");
        if (snowballs != null && snowballs.Length > 0)
        {
            for (int i = 0; i < snowballs.Length; i++)
            {
                if (snowballs[i].GetComponent<Throwable>().GetAttached())
                {
                    snowballs[i].GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
    }

    // Updates snowball tags based on whether they are attached or not to left/right controller.
    void UpdateSnowballTags()
    {
        GameObject[] enemySnowballs = GameObject.FindGameObjectsWithTag("Enemy Snowball");
        for (int i = 0; i < enemySnowballs.Length; i++)
        {
            if (enemySnowballs[i].GetComponent<Throwable>().GetAttached())
            {
                enemySnowballs[i].GetComponent<Snowball>().SetTag("Attached Snowball");
            }
        }

        GameObject[] snowballs = GameObject.FindGameObjectsWithTag("Snowball");
        for (int i = 0; i < snowballs.Length; i++)
        {
            if (snowballs[i].GetComponent<Throwable>().GetAttached())
            {
                snowballs[i].GetComponent<Snowball>().SetTag("Attached Snowball");
                //debugText.GetComponent<TextMesh>().text += "Snowball -> Attached Snowball\n";
            }
        }

        GameObject[] attachedSnowballs = GameObject.FindGameObjectsWithTag("Attached Snowball");
        for (int i = 0; i < attachedSnowballs.Length; i++)
        {
            if (!attachedSnowballs[i].GetComponent<Throwable>().GetAttached())
            {
                sfxAudioSource.clip = audioManager.GetSnowballThrow();
                sfxAudioSource.Play();
                attachedSnowballs[i].GetComponent<Snowball>().SetTag("Snowball");
                //debugText.GetComponent<TextMesh>().text += "Attached Snowball -> Snowball\n";
            }
        }
    }

    // Use this one for terrain mesh.
    void MakeSnowball()
    {
        if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.DrawRay(rightHandAnchor.transform.position, -Vector3.up, Color.red);
            RaycastHit hit;
          //  hit.distance;
            if (Physics.Raycast(rightHandAnchor.transform.position, -Vector3.up, out hit))
            {
                if (hit.transform.gameObject.tag == "Floor")
                {
                    if (hit.distance < snowHeight)
                    {   // Making a snowball from the pile with right hand.
                        sfxAudioSource.clip = audioManager.GetSnowballMake();
                        sfxAudioSource.Play();
                        Instantiate(snowballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                        SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.RightHand);
                    }
                    else if (snowballContainer != null && snowballContainer.GetComponent<SnowballContainer>().TakeSnowball())
                    {   // Taking a snowball from the container with right hand.
                        Instantiate(snowballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                        SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.RightHand);
                    }
                }
            }
        }
        if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            RaycastHit hit;
            if (Physics.Raycast(leftHandAnchor.transform.position, -Vector3.up, out hit))
            {
                if (hit.transform.gameObject.tag == "Floor")
                {
                    if (hit.distance < snowHeight)
                    {   // Making a snowball from the pile with left hand.
                        sfxAudioSource.clip = audioManager.GetSnowballMake();
                        sfxAudioSource.Play();
                        Instantiate(snowballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                        SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.LeftHand);
                    }
                    else if (snowballContainer != null && snowballContainer.GetComponent<SnowballContainer>().TakeSnowball())
                    {   // Taking a snowball from the container with left hand.
                        Instantiate(snowballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                        SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.LeftHand);
                    }
                }
            }
        }
    }

    // Use this one for flat maps.
    void MakeSnowballFlat()
    {
        if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (rightHandAnchor.transform.position.y < 0.65)
            {   // Making a snowball from the pile with right hand.
                sfxAudioSource.clip = audioManager.GetSnowballMake();
                sfxAudioSource.Play();
                Instantiate(snowballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
                SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.RightHand);
            }
        }
        if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            if (leftHandAnchor.transform.position.y < 0.65)
            {   // Making a snowball from the pile with left hand.
                sfxAudioSource.clip = audioManager.GetSnowballMake();
                sfxAudioSource.Play();
                Instantiate(snowballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
                SteamVR_Actions.default_Haptic.Execute(0, 0.5f, 20, 1, SteamVR_Input_Sources.LeftHand);
            }
        }
    }

    public void UpdateScore()
    {
        playerScore++;
        scoreText.GetComponent<TextMesh>().text = "Score: " + playerScore.ToString();
    }
}
