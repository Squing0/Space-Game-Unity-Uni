using UnityEngine;
using TMPro;
using Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager instance;

        public GameObject mainHUD;
        public GameObject winScreen;
        public GameObject gameoverScreen;

        public TMP_Text gameoverScoreText;
        public TMP_Text winScoreText;
        public TMP_Text gameoverMoralityText;
        public TMP_Text winMoralityText;

        public Slider moralitySlider;

        private PlayerMovement playerMovement;
        private Charge charger;
        private bool endSoundPlayed;

        private float totalScore;   // change to int?

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }     
            
            endSoundPlayed = false;
        }
        private void Start()
        {
            playerMovement = FindAnyObjectByType<PlayerMovement>();
            charger = FindAnyObjectByType<Charge>();
        }

        public void CalculateScore(string endReason, TMP_Text scoreText)
        {
            Time.timeScale = 0; // Pauses game

            totalScore = (100 - charger.ChargeValue) + (playerMovement.Health * 25) + (ShipManager.instance.Health * 25);

            scoreText.text = $"{endReason}\nScore: {(int)totalScore}";            
        }

        public void ActivateWin()
        {
            if (!endSoundPlayed)
            {
                endSoundPlayed = true;
                AudioManager.instance.winGameSound.Play();
            }

            winScreen.SetActive(true);
            mainHUD.SetActive(false);

            CalculateScore("You won!", winScoreText);
            CalculateMoralityText(winMoralityText);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ActivateGameover(string deathReason)
        {
            if (!endSoundPlayed)
            {
                endSoundPlayed = true;
                AudioManager.instance.LoseGameSound.Play();
            }

            gameoverScreen.SetActive(true);     // Set Background UI to active so is shown on screen
            mainHUD.SetActive(false);      // Hide main UI so that only background is shown

            CalculateScore(deathReason, gameoverScoreText);
            CalculateMoralityText(gameoverMoralityText);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void CalculateMoralityText(TMP_Text moralityText)
        {
            float value = moralitySlider.value;

            if(value > 0f && value <= 0.3f)
            {
                moralityText.text = "Good for you! You spared the innocent aliens.";
            }
            else if(value <= 0.7f)
            {
                moralityText.text = "Interesting, you spared some of the innocent aliens.";
            }
            else if(value <= 1f)
            {
                moralityText.text = "You only killed innocent aliens, you monster!";
            }
        }

        public void ResetScene()
        {
            SceneManager.LoadScene("Title Screen");
        }
    }
}