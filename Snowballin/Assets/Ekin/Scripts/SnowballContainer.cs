using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballContainer : MonoBehaviour
{
    public Vector3 distanceToPlayer;        // Distance between the container and the track target.
    public int containerSize;               // How many snowballs the container can hold. You need to modify snowballsInBucket accordingly
                                            // if you change this value.
    public GameObject[] snowballsInBucket;  // Array of snowball gameobjects inside the container (visual only).
    
    private GameObject trackTarget;         // Gameobject that the container is attached to (player head). Used for calculating x and z coordinates of the container.
    private int snowballCount;
    private GameObject debugText;           // Used for debugging while wearing the headset. It is a 3D Text attached to the head view.
    private GameObject ammoText;            // 3D Text attached to player head. Displays the number of snowballs in container.
    
    void Start()
    {
        snowballCount = 0;
        trackTarget = GameObject.FindGameObjectWithTag("MainCamera");
        debugText = GameObject.FindGameObjectWithTag("Debug Text");
        ammoText = GameObject.FindGameObjectWithTag("Ammo Text");
    }
    
    void Update()
    {
        UpdateAmmoText();
        UpdateContainerPosition();
        RenderSnowballsInContainer();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Snowball")
        {
            if (snowballCount < containerSize)
            {
                Destroy(other.gameObject);
                snowballCount++;
            }
            else
            {
                Debug.Log("Container is full.");
            }
        }
    }

    public bool TakeSnowball()
    {
        if (snowballCount > 0)
        {
            snowballCount--;
            return true;
        }
        return false;
    }

    private void UpdateAmmoText()
    {
        ammoText.GetComponent<TextMesh>().text = "Snowballs: " + snowballCount.ToString();
    }

    private void UpdateContainerPosition()
    {
        float containerY = GameObject.FindGameObjectWithTag("Player Body").transform.position.y;
        transform.position = (new Vector3(trackTarget.transform.position.x, containerY, trackTarget.transform.position.z)) - 
                             (new Vector3(distanceToPlayer.x, distanceToPlayer.y, distanceToPlayer.z));
    }

    private void RenderSnowballsInContainer()
    {
        for (int i = 0; i < containerSize; i++)
        {
            if (snowballCount >= i + 1)
            {
                snowballsInBucket[i].SetActive(true);
            }
            else
            {
                snowballsInBucket[i].SetActive(false);
            }
        }
    }
}
