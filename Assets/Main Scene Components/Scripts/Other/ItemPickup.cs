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
    public GameObject startValuesObj;   //Figure out better name for this and script
    public GameObject charger;

    private GunShoot gunshoot; // Is this best way to access other scripts?
    private PlayerMovement player;
    private HealthBar healthbar;
    private StartValues startValues;
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private Charge charge;
    private void Start()
    {
        startValues = startValuesObj.GetComponent<StartValues>();
        charge = charger.GetComponent<Charge>();
        //if(name != "Ammo" || name != "Health" || name != "Speed")
        //{
        //    StartCoroutine(DestroyPowerup());
        //}

        StartCoroutine(DestroyPowerup());
    }

    private IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(startValues.PowerupChargeAppear / charge.chargeSpeeder);    // Need to change with speed of timer
        //Destroy(startValues.CreatedPowerup);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Have UI messages for when powerups picked up?
        {
            if(tag == "AmmoIncrease")
            {
                gunshoot = other.gameObject.GetComponentInChildren<GunShoot>();
                gunshoot.AddAmmo(ammoToAdd);

                Destroy(gameObject);
            }

            if(tag == "HealthIncrease")
            {
                player = other.gameObject.GetComponent<PlayerMovement>();

                if(player.Health < player.MaxHealth)
                {
                    player.Health += 1;
                }
                else
                {
                    Debug.Log("Health full!");
                }

                healthbar = playerHealthBar.gameObject.GetComponent<HealthBar>();
                healthbar.updateHealth(player.Health, player.MaxHealth);

                Destroy(gameObject);
            }

            if(tag == "SpeedIncrease")
            {
                player = other.gameObject.GetComponent<PlayerMovement>();
                player.SpeedUpActivate(speedToAdd, speedTime);

                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (tag == "HealthIncrease")
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
                Destroy(gameObject);
            }

            if(tag == "SpeedIncrease")
            {
                ai = other.GetComponent <EnemyAI>();
                ai.SpeedUpActivate(speedToAdd, speedTime);
                Destroy(gameObject);
            }
        }
    }
}