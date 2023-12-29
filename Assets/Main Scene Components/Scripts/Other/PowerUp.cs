using TMPro;
using UnityEngine;
using Player;
public class PowerUp : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text speedText;
    public TMP_Text doubleJumpText;
    //public Revert revert;

    public PlayerMovement movement;
    public AudioSource audioSource;
    public float volume = 0.5f;
    public AudioClip clip;



    private void OnTriggerEnter(Collider other)
    {
        //    if (tag == "JumpUp")
        //    {
        //        doubleJumpText.text = "Double Jump Activated";  // Moved code from player movement to make more organised
        //        movement.maxJumps = 2;
        //        Destroy(gameObject);
        //    }

        //    if (tag == "SpeedUp")
        //    {
        //        speedText.text = "Speed Increased";
        //        movement.PowerupActivate(movement.speedIncrease);   // Used PowerupActivate specifically here as stats were changed
        //        Destroy(gameObject);
        //    }
    }
}
