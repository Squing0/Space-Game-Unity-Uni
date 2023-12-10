using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTime = 80f;
    public bool timerOn;
    public TMP_Text timerText;
    //public PlayerMovement player;
    //public GameOverScreen gameover;

    private void Start()
    {
        timerOn = true;
    }
    private void Update()
    {
        if (timerOn)
        {
            if (totalTime > 0)
            {
                totalTime -= Time.deltaTime;    // Used deltaTime to get most accurate data
                UpdateTimer();  // Timer updated every frame
            }
            else
            {
                //gameover.ActivateGameover(player.health, (int)totalTime);
            }
        }
    }

    private void UpdateTimer()
    {
        int seconds = Mathf.FloorToInt(totalTime % 60); // Used modulo to get seconds in a minute
        int minutes = Mathf.FloorToInt(totalTime / 60); // Used division to get minutes

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // used string formatting to get specific minute seconds format
    }
}
