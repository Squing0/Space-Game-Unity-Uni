using UnityEngine;
using UI;
using System.Collections;

// handles and varaibles and methods relating to the ship game object.
public class ShipManager : MonoBehaviour
{
    public static ShipManager instance; // Single instance as only one ship in scene.

    [Header("Health")]
    public int maxHealth;   
    public float healthDelay;

    [Header("Game Objects")]
    public SliderManager healthBar;

    public HealthManager healthManager;

    private bool shipAttackable;    

    private int health;

    // Property used as variable accessed in other classes.
    public int Health
    { get { return health; }}

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

        health = maxHealth; // Current health set to full.
        shipAttackable = true;  // Ship is attackable initially.
    }
    private void Start()
    {
        // Health manager instantiated to handle all health methods.
        healthManager = new HealthManager(health, maxHealth, healthBar, "Ship");
    }

    private void OnCollisionStay(Collision collision)
    {
        // Only allows enemy to attack if in collider.
        if (shipAttackable && collision.gameObject.CompareTag("Enemy"))       
        {
            StartCoroutine(ReduceShipHealth());
        }
    }

    // Bool value used along waiting so ship health is not instantly reduced to 0.
    private IEnumerator ReduceShipHealth()
    {
        if (shipAttackable) 
        {
            healthManager.DecreaseHealth(1);
            shipAttackable = false;
        }

        yield return new WaitForSeconds(healthDelay);   // Specified time to wait.

        shipAttackable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Allows player's bullets to also damage ship.
        if (other.CompareTag("Bullet"))
        {
            healthManager.DecreaseHealth(1);
        }
    }
}