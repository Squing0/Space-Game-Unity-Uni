using UnityEngine;

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
    public AudioSource powerupSound;

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
