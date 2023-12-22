using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject gunObj;
    private GunShoot gs; // Is this best way to access other scripts?
    public float ammoToAdd; // Awful name

    private void Awake()
    {
        gs = gunObj.GetComponent<GunShoot>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(tag == "AmmoIncrease")
            {
                gs.AddAmmo(ammoToAdd);
                Destroy(gameObject);
            }
        }
    }
}
