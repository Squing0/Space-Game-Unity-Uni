using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTime = 1;
    public bool timerOn;
    public TMP_Text timerText;
    private double charge = 1;
    public int timeSpeeder;

    //public GameObject gameover;
    //GameOverScreen gm;

    public GameObject win;
    GameWinScreen winScreen;

    // CHANGE SOME OF THIS OVER TO UI MANAGER

    private void Start()
    {
        timerOn = true;
        winScreen = win.GetComponent<GameWinScreen>();
    }
    private void Update()
    {
        if (charge < 100)   
        {
            charge += Time.deltaTime * timeSpeeder;
            timerText.text = $"Charge: {(int)charge}%";
        }
        else
        {
            winScreen.ActivateWin();
        }
    }
}
