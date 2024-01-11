using UnityEngine;
using UnityEngine.SceneManagement;

// Stores specific difficulty value across scenes.
public class DifficultyAcrossScenes : MonoBehaviour
{
    // Instance used so value can be crossed between scenes.
    public static DifficultyAcrossScenes instance;  

    public string difficulty;
    public string mainGameScene;

    private void Awake()
    {
        // Ensures there is only one instance or object is destroyed.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Difficulty is set and main game is loaded.
    public void SelectDifficulty(string difficultyChoice)
    {
        instance.difficulty = difficultyChoice;
        SceneManager.LoadScene(mainGameScene);
    }
}
