using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{  
    public class TitleScreen : MonoBehaviour
    {
        public GameObject difficultyScreen;
        public void StartBtnClick()
        {
            //SceneManager.LoadScene("Level1");   // Change name!
            difficultyScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ExitBtnClick()
        {
//            Debug.Log("Exit Clicked!");
//#if UNITY_EDITOR
//            EditorApplication.ExitPlaymode();
//#else
//            Application.Quit();               // Technically this gpt code is more accurate but need to make more unique or reference from elsewhere
//#endif


            EditorApplication.ExitPlaymode();
            Application.Quit();
        }
    }
}