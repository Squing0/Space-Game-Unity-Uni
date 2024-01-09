using System.Collections;
using UnityEngine;
using Player;
using UI;
using Enemy;

public class ItemPickup : MonoBehaviour
{
    [Header("Powerups")]
    public int ammoToAdd; 
    public int healthToAdd;
    public int speedToAdd;
    public float speedTime;

    [Header("Other Objects")]
    public GameObject playerHealthBar;    // Don't like this as speed and ammo don't use

    private GunShoot gunshoot; 
    private PlayerMovement player;
    private StartValues startValues;
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private Charge charge;
    private void Start()
    {
        startValues = FindAnyObjectByType<StartValues>();
        charge = FindAnyObjectByType<Charge>();

        StartCoroutine(DestroyPowerup());
    }

    private IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(startValues.PowerupChargeAppear / charge.ChargeSpeeder);   
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if(gameObject.CompareTag("AmmoIncrease"))
            {
                gunshoot = other.gameObject.GetComponentInChildren<GunShoot>();
                gunshoot.AddAmmo(ammoToAdd);
            }

            if(gameObject.CompareTag("HealthIncrease"))
            {
                player = other.gameObject.GetComponent<PlayerMovement>();

                if(player.Health < player.MaxHealth)
                {
                    player.IncreaseHealth(1);
                }
                else
                {
                    Debug.Log("Health full!");
                }
            }

            if(gameObject.CompareTag("SpeedIncrease"))
            {
                player = other.gameObject.GetComponent<PlayerMovement>();
                player.SpeedUpActivate(speedToAdd, speedTime);
            }

            AudioManager.instance.powerupSound.Play();
        }

        if (other.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("HealthIncrease"))
            {
                enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats.Health < enemyStats.MaxHealth)   // repeated from above
                {
                    enemyStats.IncreaseHealth(1);
                }
                else
                {
                    Debug.Log("Health full!");
                }              
            }

            if(gameObject.CompareTag("SpeedIncrease"))
            {
                ai = other.GetComponent<EnemyAI>();
                ai.SpeedUpActivate(speedToAdd, speedTime);
            }

        }

        Destroy(gameObject);
    }

    private void HealthActivate()
    {

    }
}