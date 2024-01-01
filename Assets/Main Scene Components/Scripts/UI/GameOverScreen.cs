using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public TMP_Text scoreText;
        public GameObject gameoverScreen;
        public GameObject mainUIScreen;
        public GameObject UiManagerObj;

        private UiManager uiManager;

        private void Start()
        {
            uiManager = UiManagerObj.GetComponent<UiManager>();
        }

        public void ActivateGameover(string deathReason)
        {
            gameoverScreen.SetActive(true);     // Set Background UI to active so is shown on screen
            mainUIScreen.SetActive(false);      // Hide main UI so that only background is shown

            uiManager.CalculateScore(deathReason, scoreText);   
        }

        public void ResetScene()
        {
            SceneManager.LoadScene("Title Screen");
        }
    }
}