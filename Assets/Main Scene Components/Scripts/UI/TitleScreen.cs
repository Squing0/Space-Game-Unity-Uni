using UnityEditor;
using UnityEngine;

namespace UI
{  
    public class TitleScreen : MonoBehaviour
    {
        public GameObject difficultyScreen;
        public GameObject titleScreen;
        public GameObject controlsScreen;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None; // Cursor needs to be reset when game is reloaded
            Cursor.visible = true;
        }
        public void StartBtnClick()
        {
            difficultyScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ExitBtnClick()
        {
            EditorApplication.ExitPlaymode();
            Application.Quit();
        }

        public void BackBtnClick()
        {
            controlsScreen.SetActive(false);
            titleScreen.SetActive(true);          
        }

        public void ControlBtnClick()
        {
            titleScreen.SetActive(false);
            controlsScreen.SetActive(true);
        }
    }
}