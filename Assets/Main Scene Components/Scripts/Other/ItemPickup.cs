using System.Collections;
using UnityEngine;
using Player;
using UI;
using Enemy;

// Handles effects of powerups.
public class ItemPickup : MonoBehaviour
{
    [Header("Powerups")]
    public int ammoToAdd; 
    public int healthToAdd;
    public int speedToAdd;
    public float speedTime; // How long speed powerup will last.

    [Header("Other Objects")]
    public GameObject playerHealthBar;      // Player health bar for health powerup.

    // Scripts that will be used.
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

        StartCoroutine(DestroyPowerup());   // Powerup appears for limited time.
    }

    // How long powerup last until destroyed if not picked up. Changes depending on speed of charge.
    private IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(startValues.PowerupChargeAppear / charge.ChargeSpeeder);   
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if object entered trigger is player.
        {
            if(gameObject.CompareTag("AmmoIncrease"))   // If ammo powerup, increase player ammo.
            {
                gunshoot = other.GetComponentInChildren<GunShoot>();
                gunshoot.AddAmmo(ammoToAdd);
            }

            if(gameObject.CompareTag("HealthIncrease")) // If health powerup, increase player health.
            {
                player = other.GetComponent<PlayerMovement>();

                if(player.Health < player.MaxHealth)    // Only increases health if less than max health.
                {
                    player.healthManager.IncreaseHealth(1);
                }
            }

            if(gameObject.CompareTag("SpeedIncrease"))  // If speed powerup, increases player speed with specified amount and time.
            {
                player = other.GetComponent<PlayerMovement>();
                player.SpeedUpActivate(speedToAdd, speedTime);
            }

            AudioManager.instance.powerupSound.Play();  // Only plays powerup sound if player enters trigger.
        }

        if (other.CompareTag("Enemy")) // Checks if object entered trigger is enemy.
        {
            if (gameObject.CompareTag("HealthIncrease"))    // If health powerup, increase enemy health.
            {
                enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats.Health < enemyStats.MaxHealth)   // Only increases health if less than max health.
                {
                    enemyStats.healthManager.IncreaseHealth(1);
                }          
            }

            if(gameObject.CompareTag("SpeedIncrease")) // If speed powerup, increases enemy speed with specified amount and time.
            {
                ai = other.GetComponent<EnemyAI>();
                ai.SpeedUpActivate(speedToAdd, speedTime);
            }

        }

        Destroy(gameObject);    // Powerup destroyed if picked up.
    }

    //private void HealthActivate(Collider collider, ) Tried to automate
    //{
    //    enemyStats = other.GetComponent<EnemyStats>();
    //    if (enemyStats.Health < enemyStats.MaxHealth)   // repeated from above
    //    {
    //        enemyStats.healthManager.IncreaseHealth(1);
    //    }
    //}
}