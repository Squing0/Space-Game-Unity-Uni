using UnityEngine;
using UI;
using System;
using System.Collections;

public class ShipManager : MonoBehaviour
{
    [Header("Health")]
    private int health;
    public int maxHealth;
    public float healthDelay;

    [Header("Game Objects")]
    public HealthBar healthBar;
    public GameObject UIManager;
    private UiManager UI;

    private bool shipAttackable;
    public int Health
    {
        get { return health; }
    }

    private void Start()
    {
        UI = UIManager.GetComponent<UiManager>();
        health = maxHealth;
        shipAttackable = true;
    }
    private void Update()
    {
        if (health < 1)
        {
            UI.ActivateGameover("Your ship was destroyed!");
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && shipAttackable)       // Change to compare tag for this and other occurences
        {
            StartCoroutine(ReduceShipHealth());
        }
    }

    private IEnumerator ReduceShipHealth()
    {
        if (shipAttackable) // Chatgpt modified original code (don't think needs to be changed honestly)
        {
            health--;
            healthBar.updateHealth(health, maxHealth);
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
            healthBar.updateHealth(health, maxHealth);
        }
    }
}
