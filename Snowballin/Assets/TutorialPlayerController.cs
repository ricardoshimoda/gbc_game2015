using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class TutorialPlayerController : MonoBehaviour
{

    public GameObject waveCompleteText;

    public AudioClip success;

    public GameObject controller;

    public Snowman snowman;

    private AudioSource sfxAudioSource;

    private int TutorialState = 0; 

    private void Start()
    {
        sfxAudioSource = GameObject.Find("Audio SFX").GetComponent<AudioSource>();
        Invoke("Tutorial_One", 3);
    }

    void Update()
    {
        var snowballInHand = GameObject.FindGameObjectsWithTag("Attached Snowball");
        var snowmanCount = GameObject.FindGameObjectsWithTag("Snowman");
        if(TutorialState == 2 && snowballInHand != null && snowballInHand.Length > 0){
            waveCompleteText.GetComponent<Text>().text = "Congratulations!";
            controller.SetActive(false);
            Invoke("TutorialTextOff", 2);
            TutorialState = 3;
        }
        if(TutorialState == 4 && (snowballInHand == null || snowballInHand.Length <= 0)){
            waveCompleteText.GetComponent<Text>().text = "Congratulations!";
            controller.SetActive(false);
            Invoke("TutorialTextOff", 2);
            TutorialState = 5;
            return;
        }
        
        if(TutorialState == 6 && (snowmanCount == null || snowmanCount.Length <= 0)){
            waveCompleteText.GetComponent<Text>().text = "Congratulations!";
            controller.SetActive(false);
            Invoke("GoToNextLevel", 2);
            return;
        }
    }
    
    public void Tutorial_One(){
        waveCompleteText.GetComponent<Text>().text = "Welcome to SNOWBALLIN'";
        waveCompleteText.SetActive(true);
        TutorialState = 1;
        Invoke("TutorialTextOff", 10);
    }

    public void Tutorial_Two(){
        waveCompleteText.GetComponent<Text>().text = "To create a snowball, immerse your hand in the snow and press the grab trigger";
        waveCompleteText.SetActive(true);
        controller.SetActive(true);
        TutorialState = 2;
    }
    public void Tutorial_Three(){
        waveCompleteText.GetComponent<Text>().text = "To throw a snowball, just release the trigger while moving your hand and arm";
        waveCompleteText.SetActive(true);
        controller.SetActive(true);
        TutorialState = 4;
    }

    public void Tutorial_Four(){
        waveCompleteText.GetComponent<Text>().text = "Now hit this creepy snowman to start the game!";
        waveCompleteText.SetActive(true);
        snowman.blinking = true;
        TutorialState = 6;
    }

    void TutorialTextOff()
    {
        waveCompleteText.SetActive(false);
        if(TutorialState == 1){
            Invoke("Tutorial_Two", 3);
        }
        else if(TutorialState == 3){
            Invoke("Tutorial_Three", 3);
        }
        else if(TutorialState == 5){
            Invoke("Tutorial_Four", 3);
        }
    }

    void GoToNextLevel(){
        SceneManager.LoadScene("Beta");
    }


}
