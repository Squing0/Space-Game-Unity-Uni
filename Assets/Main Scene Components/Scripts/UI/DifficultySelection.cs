using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DifficultySelection : MonoBehaviour
    {
        private Difficulty difficulty;

        public void SelectDifficulty(string difficultyChoice)
        {
            DifficultyAcrossScenes.instance.difficulty = difficultyChoice;
            SceneManager.LoadScene("Level1");
        }

    }
}