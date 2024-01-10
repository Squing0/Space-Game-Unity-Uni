using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyAcrossScenes : MonoBehaviour
{
    public static DifficultyAcrossScenes instance;  // Mention could have used playerprefs
    public string difficulty;
    public string mainGameScene;

    private void Awake()
    {
        instance = this;    // may need to add checking if null but not sure
    }

    public void SelectDifficulty(string difficultyChoice)
    {
        instance.difficulty = difficultyChoice;
        SceneManager.LoadScene(mainGameScene);
    }
}
