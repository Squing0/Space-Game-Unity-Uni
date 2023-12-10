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

    public GameObject gameover;
    GameOverScreen gm;

    // CHANGE SOME OF THIS OVER TO UI MANAGER

    private void Start()
    {
        timerOn = true;
        gm = gameover.GetComponent<GameOverScreen>();
    }
    private void Update()
    {
        if (charge < 100)
        {
            charge += Time.deltaTime * 5;
            timerText.text = $"Charge: {(int)charge}%";
        }
        else
        {
            gm.ActivateGameover(3, 3);
        }
    }
}
