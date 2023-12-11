using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinScreen : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject mainUI;
    public void ActivateWin()
    {
        winScreen.SetActive(true);
        mainUI.SetActive(false);
    }
}