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
        public string mainMenuScene;

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

            totalScore = (100 - charger.ChargeValue) + (playerMovement.Health * 25) + (ShipManager.instance.healthManager.health * 25);

            scoreText.text = $"{endReason}\nScore: {(int)totalScore}";            
        }

        public void ActivateWin()
        {
            ActivateEndGame(AudioManager.instance.winGameSound, 
                winScreen, "You won!", winScoreText, 
                winMoralityText);
        }

        public void ActivateGameover(string deathReason)
        {
            ActivateEndGame(AudioManager.instance.LoseGameSound, 
                gameoverScreen, deathReason,gameoverScoreText, 
                gameoverMoralityText);
        }

        public void ActivateEndGame(AudioSource endSound, GameObject endScreen, string scoreTextValue, TMP_Text scoreText,  TMP_Text moralityText)
        {
            if (!endSoundPlayed)
            {
                endSoundPlayed = true;
                endSound.Play();
            }

            endScreen.SetActive(true);
            mainHUD.SetActive(false);

            CalculateScore(scoreTextValue, scoreText);
            CalculateMoralityText(moralityText);

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
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}