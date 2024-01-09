using Assets.Main_Scene_Components.Scripts.Other;
using UI;
using UnityEngine;

public class HealthManager : HealthMethods
{
    private int health;
    private int maxHealth;
    private GameObject healthBarObj;
    private HealthBar healthBar;

    public HealthManager(int hea, int maxHea, HealthBar heaBar)
    {
        health = hea;
        maxHealth = maxHea;
        healthBar = heaBar;
        //healthBar = healthBarObj.GetComponent<HealthBar>();
    }
    //    public Health(int health, int maxHealth)
    //    {
    //        this.health = health;
    //        this.maxHealth = maxHealth;
    //    }

    //    public void reduceHealth(int damage)
    //    {
    //        health -= damage;
    //        healthBar.updateHealth(health, maxHealth);
    //    }

    //    public void IncreaseHealth(int amount)
    //    {
    //        health += amount;
    //        healthBar.updateHealth(health, maxHealth);
    //    }

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
