using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI speedrunTimer;
    [SerializeField]private TextMeshProUGUI finalTime;
    [SerializeField]private TextMeshProUGUI deathCount;
    [SerializeField]private TextMeshProUGUI victoryCongrats;
    [SerializeField]private GameObject panel;
    private TimeSpan timePlaying;
    private float elaspedTime;
    private bool timerGoing;
    [SerializeField]private Shadow player;
    
    
    void Start(){
        speedrunTimer.text = "00:00:00.00";
        elaspedTime = 0f;
        timerGoing = true;
    }
    void Update()
    {
        if(timerGoing){
            elaspedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elaspedTime);
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss'.'ff");
            speedrunTimer.text = timePlayingStr;
        }
    }

    void OnTriggerEnter2D(){
        timerGoing = false;
        speedrunTimer.text = "";
        deathCount.text = "You died " + player.deathCounter.ToString() + " times";
        finalTime.text = "Final Time: " + timePlaying.ToString("hh':'mm':'ss'.'ff");
        panel.SetActive(true);
        victoryCongrats.enabled = true;
    }
}
