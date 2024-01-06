using UnityEngine;
using UI;
using System;
using System.Collections;

public class ShipManager : MonoBehaviour
{
    [Header("Health")]
    public int health = 20;
    public int maxHealth = 20;
    public float healthDelay;

    [Header("Game Objects")]
    public GameObject gameover;
    public HealthBar healthBar;

    //private GameOverScreen gm;
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
