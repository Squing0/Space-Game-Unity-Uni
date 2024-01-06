using UnityEngine;
using TMPro;
using Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject playerObj;
        public GameObject timerObj;
        public GameObject shipObj;
        public GameObject mainHUD;
        public TMP_Text gameoverScoreText;
        public TMP_Text winScoreText;
        public GameObject winScreen;
        public GameObject gameoverScreen;
        public Button gameoverRestart;
        public Button winRestart;

        private PlayerMovement playerMovement;
        private Charge charger;
        private ShipManager shipManager; // CHANGE FOR THE LOVE OF GOD

        private float totalScore;   // change to int?
        private void Start()
        {
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            charger = timerObj.GetComponent<Charge>();
            shipManager = shipObj.GetComponent<ShipManager>();
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.R) && mainHUD.activeInHierarchy == false)    // Allows the user to restart with R as mouse can't be used due to it being used with the camera
            //{
            //    restart.onClick.Invoke();
            //}
        }

        public void CalculateScore(string endReason, TMP_Text scoreText)
        {
            charger.ChargeOn = false;
            // Make stopper for ship health and player health too? don't want to repeate same thing three times

            totalScore = (100 - charger.ChargeValue) + (playerMovement.Health * 25) + (shipManager.Health * 25);
            scoreText.text = $"{endReason}\nScore: {(int)totalScore}";
        }

        public void ActivateWin()
        {
            winScreen.SetActive(true);
            mainHUD.SetActive(false);

            CalculateScore("You won!", winScoreText);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ActivateGameover(string deathReason)
        {
            gameoverScreen.SetActive(true);     // Set Background UI to active so is shown on screen
            mainHUD.SetActive(false);      // Hide main UI so that only background is shown

            CalculateScore(deathReason, gameoverScoreText);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResetScene()
        {
            SceneManager.LoadScene("Title Screen");
        }
    }
}