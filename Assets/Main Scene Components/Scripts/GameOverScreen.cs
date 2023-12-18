using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameoverScreen;
    public GameObject mainUIScreen;

    public void ActivateGameover(float health, int timeLeft, string deathReason)
    {
        gameoverScreen.SetActive(true);     // Set Background UI to active so is shown on screen
        mainUIScreen.SetActive(false);      // Hide main UI so that only background is shown
        float totalScore = timeLeft + (health * 25);    // Total score calcualted. Health increased to lower disparity in comaprison to time
        scoreText.text = $"Score: {totalScore}\n{deathReason}";
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
