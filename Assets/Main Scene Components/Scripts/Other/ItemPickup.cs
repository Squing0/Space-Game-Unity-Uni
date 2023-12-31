using System.Collections;
using UnityEngine;
using Player;
using UI;
using Enemy;

public class ItemPickup : MonoBehaviour
{
    public int ammoToAdd; // Awful name
    public int healthToAdd;
    public int speedToAdd;
    public float speedTime;

    public GameObject playerHealthBar;    // Don't like this as speed and ammo don't use
    public GameObject gameSetter;   //Figure out better name for this and script

    private GunShoot gs; // Is this best way to access other scripts?
    private PlayerMovement pm;
    private HealthBar hb;
    private EnemyCreator en;
    private EnemyStats es;
    private BasicAi ai;
    private void Start()
    {
        en = gameSetter.GetComponent<EnemyCreator>();
        StartCoroutine(DestroyPowerup());     
    }

    private IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(en.PowerupChargeAppear);    // Need to change with speed of timer
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Have UI messages for when powerups picked up?
        {
            if(tag == "AmmoIncrease")
            {
                gs = other.gameObject.GetComponentInChildren<GunShoot>();
                gs.AddAmmo(ammoToAdd);
                Destroy(gameObject);
            }

            if(tag == "HealthIncrease")
            {
                pm = other.gameObject.GetComponent<PlayerMovement>();

                if(pm.Health < pm.MaxHealth)
                {
                    pm.Health += 1;
                }
                else
                {
                    Debug.Log("Health full!");
                }

                Debug.Log(pm.Health);
                hb = playerHealthBar.gameObject.GetComponent<HealthBar>();
                hb.updateHealth(pm.Health, pm.MaxHealth);
                Destroy(gameObject);
            }

            if(tag == "SpeedIncrease")
            {
                pm = other.gameObject.GetComponent<PlayerMovement>();
                pm.SpeedUpActivate(speedToAdd, speedTime);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (tag == "HealthIncrease")
            {
                es = other.GetComponent<EnemyStats>();
                if (es.Health < es.MaxHealth)   // repeated from above
                {
                    es.IncreaseHealth(1);
                }
                else
                {
                    Debug.Log("Health full!");
                }              
                Destroy(gameObject);
            }

            if(tag == "SpeedIncrease")
            {
                ai = other.GetComponent <BasicAi>();
                ai.SpeedUpActivate(speedToAdd, speedTime);
                Destroy(gameObject);
            }
        }
    }
}