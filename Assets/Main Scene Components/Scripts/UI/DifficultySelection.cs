using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DifficultySelection : MonoBehaviour
    {
        public void SelectDifficulty(string difficultyChoice)
        {
            DifficultyAcrossScenes.instance.difficulty = difficultyChoice;
            SceneManager.LoadScene("Main Game");
        }

    }
}