using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource startScreenMusic;
    public AudioSource mainGameMusic;
    public AudioSource shootSound;
    public AudioSource reloadSound;
    public AudioSource winGameSound;
    public AudioSource LoseGameSound;
    public AudioSource enemyDeathSound;   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
