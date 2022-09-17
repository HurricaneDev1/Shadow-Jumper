using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI speedrunTimer;
    private TimeSpan timePlaying;
    private float elaspedTime;
    
    void Start(){
        speedrunTimer.text = "00:00:00.00";
        elaspedTime = 0f;
    }
    void Update()
    {
        elaspedTime += Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elaspedTime);
        string timePlayingStr = timePlaying.ToString("hh':'mm':'ss'.'ff");
        speedrunTimer.text = timePlayingStr;
    }
}
