using UnityEngine;

// Stores all audio sources.
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    // Single instance so easy to access audio from any class.

    [Header("Background Music")]
    public AudioSource startScreenMusic;
    public AudioSource mainGameMusic;

    [Header("Sound effects")]
    public AudioSource shootSound;
    public AudioSource reloadSound;
    public AudioSource winGameSound;
    public AudioSource LoseGameSound;
    public AudioSource enemyDeathSound;   
    public AudioSource powerupSound;

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
}
