using UnityEngine;
using TMPro;

namespace UI
{
    public class GameWinScreen : MonoBehaviour
    {
        public GameObject winScreen;
        public GameObject mainUI;
        public TMP_Text scoreText;
        public GameObject UiManagerObj;

        private UiManager uiManager;

        private void Start()
        {
            uiManager = UiManagerObj.GetComponent<UiManager>();
        }
        public void ActivateWin()
        {
            winScreen.SetActive(true);
            mainUI.SetActive(false);

            uiManager.CalculateScore("You won!", scoreText);
        }
    }
}