using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
    public int health = 3;
    public int maxHealth = 3;

    public EnemyHealthBar healthBar;    // Change this to be more efficient
    public int Health
    {
        get { return health; } set { health = value; }  // Use MAX health here too
    }

    public int MaxHealth
    {
        get { return maxHealth; } set { maxHealth = value; }
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }
    private void Update()
    {
        if(health < 1)
        {
            Destroy(gameObject);
        }
    }

    public void reduceHealth(int damage)
    {
        health -= damage;
        healthBar.updateHealth(health, maxHealth);
    }
}
