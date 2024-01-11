using UnityEngine;
using TMPro;
using Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    // Manages multiple UI elements within main game.
    public class UiManager : MonoBehaviour
    {
        public static UiManager instance;   // Single instance as only one UI manager object.

        // UI Objects that will be changed.
        [Header("UI Objects")]
        public GameObject mainHUD;
        public GameObject winScreen;
        public GameObject gameoverScreen;

        // Text elements for win and gameover screen
        [Header("Text objects")]
        public TMP_Text gameoverScoreText;
        public TMP_Text winScoreText;
        public TMP_Text gameoverMoralityText;
        public TMP_Text winMoralityText;

        [Header("Others")]
        public Slider moralitySlider;
        public string mainMenuScene;    // Made as variable in case name changes.

        // Scripts that will be changed.
        private PlayerMovementAndStats playerMovement;  
        private Charge charger;

        private bool endSoundPlayed;
        private float totalScore;   

        private void Awake()
        {
            // Ensures there is only one instance or object is destroyed.
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
            // Other scripts are found in scene
            playerMovement = FindAnyObjectByType<PlayerMovementAndStats>();
            charger = FindAnyObjectByType<Charge>();
        }

        // Score is calcualted for win and lose screen based on charge left, player health and ship health
        public void CalculateScore(string endReason, TMP_Text scoreText)
        {
            Time.timeScale = 0; // Pauses game

            totalScore = (100 - charger.ChargeValue) + 
                (playerMovement.healthManager.health * 25) + // Change depending on health manager working
                (ShipManager.instance.healthManager.health * 25);

            scoreText.text = $"{endReason}\nScore: {(int)totalScore}";            
        }

        // Gets win varables to input for end game method.
        public void ActivateWin()
        {
            ActivateEndGame(AudioManager.instance.winGameSound, 
                winScreen, "You won!", winScoreText, 
                winMoralityText);
        }

        // Gets gameover varables to input for end game method.
        public void ActivateGameover(string deathReason)
        {
            ActivateEndGame(AudioManager.instance.LoseGameSound, 
                gameoverScreen, deathReason,gameoverScoreText, 
                gameoverMoralityText);
        }

        // Shows ened screen along with score and morality text.
        public void ActivateEndGame(AudioSource endSound, GameObject endScreen, string scoreTextValue, TMP_Text scoreText,  TMP_Text moralityText)
        {
            if (!endSoundPlayed)
            {
                endSoundPlayed = true;
                endSound.Play();    // Bool used to ensure sound effect only plays once as method is continuously running.
            }

            endScreen.SetActive(true);
            mainHUD.SetActive(false);   // Main screen set to false so no overlap

            CalculateScore(scoreTextValue, scoreText);
            CalculateMoralityText(moralityText);

            Cursor.lockState = CursorLockMode.None; // Cursor is unlocked and set to visible so button is selectable.
            Cursor.visible = true;
        }

        // Sets different morality texts depending on where slider value is within range.
        public void CalculateMoralityText(TMP_Text moralityText)
        {
            float value = moralitySlider.value;

            if(value > 0f && value <= 0.3f) // Low morality.
            {
                moralityText.text = "Good for you! You spared the innocent aliens.";
            }
            else if(value <= 0.7f)  // Medium morality.
            {
                moralityText.text = "Interesting, you spared some of the innocent aliens.";
            }
            else if(value <= 1f)    // High morality.
            {
                moralityText.text = "You only killed innocent aliens, you monster!";
            }
        }

        // Calls main game scene.
        public void ResetScene()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}