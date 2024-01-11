using UnityEditor;
using UnityEngine;

namespace UI
{  
    // Manages UI elements within main menu.
    public class TitleScreen : MonoBehaviour
    {
        // Different UI elements
        public GameObject difficultyScreen;
        public GameObject titleScreen;
        public GameObject controlsScreen;

        // Shows difficulty screen and hides main screen.
        public void StartBtnClick()
        {
            difficultyScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        // Exits editor and game.
        public void ExitBtnClick()
        {
            EditorApplication.ExitPlaymode();
            Application.Quit();
        }

        // Shows controls screen and hides main screen.
        public void ControlBtnClick()
        {
            titleScreen.SetActive(false);
            controlsScreen.SetActive(true);
        }

        // Shows main screen and hides controls screen.
        public void BackBtnClick()
        {
            controlsScreen.SetActive(false);
            titleScreen.SetActive(true);          
        }
    }
}