using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyAcrossScenes : MonoBehaviour
{
    public static DifficultyAcrossScenes instance;  // Mention could have used playerprefs
    public string difficulty;

    private void Awake()
    {
        //if (instance == null)   // GOT FROM GPT
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        instance = this;
    }

    public void SelectDifficulty(string difficultyChoice)
    {
        instance.difficulty = difficultyChoice;
        SceneManager.LoadScene("Main Game");
    }
}
