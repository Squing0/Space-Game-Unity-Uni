using UnityEngine;
using UI;
using System.Collections;

public class ShipManager : MonoBehaviour
{
    public static ShipManager instance; // Anything relating to an instance needs to be kept in awake and not update

    [Header("Health")]
    private int health;
    public int maxHealth;
    public float healthDelay;

    [Header("Game Objects")]
    public HealthBar healthBar;

    private bool shipAttackable;
    public int Health
    { get { return health; }}

    private void Awake()
    {
        if(instance == null)    // Took from gpt but keep for everything else too
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        health = maxHealth;
        shipAttackable = true;
    }
    private void Start()
    {
        //healthBar = new HealthBar(health, maxHealth);
    }
    private void Update()
    {
        if (health < 1)
        {
            UiManager.instance.ActivateGameover("Your ship was destroyed!");
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && shipAttackable)       
        {
            StartCoroutine(ReduceShipHealth());
        }
    }

    private IEnumerator ReduceShipHealth()
    {
        if (shipAttackable) // Chatgpt modified original code (don't think needs to be changed honestly)
        {
            DecreaseHealth(1);
            shipAttackable = false;
        }

        yield return new WaitForSeconds(healthDelay);

        shipAttackable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            DecreaseHealth(1);
        }
    }

    public void DecreaseHealth(int damage)
    {
        health -= damage;
        healthBar.UpdateHealth(health, maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        healthBar.UpdateHealth(health, maxHealth);
    }
}
