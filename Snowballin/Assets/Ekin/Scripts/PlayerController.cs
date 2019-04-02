using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private int health;                             // Player health.

    public bool godMode;                            // True: God Mode ON. False: God Mode OFF.
    public int snowballDamage;                      // Snowball damage.
    public int regenPerTick;                        // Amount of health regenerated each tick.
    public float regenFrequency;                    // Frequency of the regeneration.
    public float startRegenAfter;                   // Player will start to regenerate health after this many seconds.
    public Image image1, image2, image3, image4;    // Images that are displayed when the player takes damage.
    public GameObject gameOverText;                 // Text that says "Game Over".
    public GameObject levelCompleteText;            // Text that says "Level Complete".
    public GameObject waveCompleteText;             // Text that says "Wave Complete".

    private bool playerDead;

    private void Start()
    {
        health = 4;
        playerDead = false;
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        image3.gameObject.SetActive(false);
        image4.gameObject.SetActive(false);
        gameOverText.SetActive(false);
        levelCompleteText.SetActive(false);
        waveCompleteText.SetActive(false);
    }

    void Update()
    {
        if (!playerDead)
        {
            UpdatePlayerCanvas();
        }
    }
    
    void UpdatePlayerCanvas()
    {
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        image3.gameObject.SetActive(false);

        if (health < 4)
        {
            image1.gameObject.SetActive(true);
        }
        if (health < 3)
        {
            image2.gameObject.SetActive(true);
        }
        if (health < 2)
        {
            image3.gameObject.SetActive(true);
        }
        if (health < 1 && !godMode)
        {
            image4.gameObject.SetActive(true);
            playerDead = true;
            gameOverText.SetActive(true);
            Invoke("GameOver", 3);
        }
    }

    void RegenerateHealth()
    {
        health += regenPerTick;
        if (health > 4)
        {
            health = 4;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy Snowball")
        {
            Debug.Log("Hit");
            CancelInvoke("RegenerateHealth");
            SteamVR_Actions.default_Haptic.Execute(0, 1f, 20, 1, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions.default_Haptic.Execute(0, 1f, 20, 1, SteamVR_Input_Sources.LeftHand);
            health -= snowballDamage;
            InvokeRepeating("RegenerateHealth", startRegenAfter, regenFrequency);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("Beta");
        Start();
    }

    public void LevelComplete()
    {
        levelCompleteText.SetActive(true);
        Invoke("GameOver", 3);
    }

    public void WaveComplete(int waveNo)
    {
        waveCompleteText.GetComponent<Text>().text = "WAVE " + waveNo.ToString() + " COMPLETE";
        waveCompleteText.SetActive(true);
        Invoke("WaveCompleteTextOff", 3);
    }

    void WaveCompleteTextOff()
    {
        waveCompleteText.SetActive(false);
    }

}
