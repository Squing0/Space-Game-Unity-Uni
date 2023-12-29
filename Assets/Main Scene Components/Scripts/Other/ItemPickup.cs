using System.Collections;
using UnityEngine;
using Player;

public class ItemPickup : MonoBehaviour
{
    public int ammoToAdd; // Awful name
    public int healthToAdd;
    public int speedToAdd;
    public float speedTime;

    private GunShoot gs; // Is this best way to access other scripts?
    private PlayerMovement pm;
    private HealthBar hb;
    private void Start()
    {
        StartCoroutine(DestroyPowerup());
    }

    private IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(10);    // Make variable (get from creator script so consistent?)
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
                Debug.Log(pm.Health);
                //hb = other.gameObject.GetComponentInChildren<HealthBar>();
                //hb.updateHealth(pm.Health, pm.MaxHealth);
                Destroy(gameObject);
            }

            if(tag == "SpeedIncrease")
            {
                pm = other.gameObject.GetComponent<PlayerMovement>();
                pm.SpeedUpActivate(speedToAdd, speedTime);
                Destroy(gameObject);
            }
        }
    }
}
