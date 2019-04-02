using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public float sizeIncreaseAmount;    // The size of the snowball will increase by this much everytime it collides with another snowball.
    public float consumeAmount;         // Snowball will be scaled down by this much ever tick during fort building.
    public float consumeLimit;          // When the snowball becomes smaller than this, it cannot be used to build a fort.

    private GameObject debugText;       // Used for debugging while wearing the headset. It is a 3D Text attached to the head view.
    private GameObject snowPile;        // Used for updating player score when a target is hit.
    private int size;                   // Current size of the snowball;    
    private bool isConsumed;            // A consumed snowball cannot be used to build forts.
    private AudioSource audioSource;    // Reference to the audio source.
    private AudioManager audioManager;  // Reference to the audio manager.

    void Start()
    {
        isConsumed = false;
        size = 1;
        debugText = GameObject.FindGameObjectWithTag("Debug Text");
        snowPile = GameObject.FindGameObjectWithTag("Snow Pile");
        audioSource = GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Snowball")
        {
            if (gameObject.tag == "Attached Snowball")
            {
                IncreaseSize(collision.gameObject.GetComponent<Snowball>().GetSize());
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && gameObject.tag != "Attached Snowball")
        {
            snowPile.GetComponent<SnowballPuff>().StartPuffAnimation(gameObject.transform);
            audioSource.clip = audioManager.GetSnowballHit(0);
            audioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
        if (collision.gameObject.tag == "Environment" && gameObject.tag != "Attached Snowball" ||
            collision.gameObject.tag == "Fort" && gameObject.tag != "Attached Snowball")
        {
            snowPile.GetComponent<SnowballPuff>().StartPuffAnimation(gameObject.transform);
            audioSource.clip = audioManager.GetSnowballHit(1);
            audioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }

        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy Snowball")
        {
            snowPile.GetComponent<SnowballPuff>().StartPuffAnimation(gameObject.transform);
            audioSource.clip = audioManager.GetSnowballHit(2);
            audioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }

        if ((collision.gameObject.tag == "Target" || collision.gameObject.tag == "Snowman" )&& gameObject.tag == "Snowball")
        {
            snowPile.GetComponent<SnowballPuff>().StartPuffAnimation(gameObject.transform);
            snowPile.GetComponent<SnowPile>().UpdateScore();
            audioSource.clip = audioManager.GetSnowballHit(2);
            audioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }

        if (collision.gameObject.tag == "Tree" && gameObject.tag == "Snowball")
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Shake", true);
            snowPile.GetComponent<SnowballPuff>().StartPuffAnimation(gameObject.transform);
            audioSource.clip = audioManager.GetSnowballHit(1);
            audioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }

    public void IncreaseSize(int multiplier)
    {
        transform.localScale += new Vector3(sizeIncreaseAmount, sizeIncreaseAmount, sizeIncreaseAmount) * multiplier;
        size++;
    }

    public int GetSize()
    {
        return size;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
    }

    public void ConsumeSnowball()
    {
        if (!isConsumed)
        {
            transform.localScale -= new Vector3(consumeAmount, consumeAmount, consumeAmount);
            if (transform.localScale.x <= consumeLimit)
            {
                isConsumed = true;
            }
        }
    }

    public bool Consumed()
    {
        return isConsumed;    
    }
}
