using UnityEngine;

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
}
