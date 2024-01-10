using Assets.Main_Scene_Components.Scripts.Other;
using Enemy;
using UI;
using UnityEditor;
using UnityEngine;

public class HealthManager : HealthMethods
{
    public int health;
    private int maxHealth;
    private SliderManager healthBar;
    private string type;
    private EnemyStats enemyStats;

    public HealthManager(int hea, int maxHea, SliderManager heaBar, string type)
    {
        health = hea;
        maxHealth = maxHea;
        healthBar = heaBar;
        this.type = type;
    }
    public void DecreaseHealth(int damage)
    {
        health -= damage;
        healthBar.UpdateHealth(health, maxHealth);

        if(health < 1)
        {
            gameEnd(type);
        }
    }

    private void gameEnd(string type)
    {
        switch (type)
        {
            case "Ship":
                UiManager.instance.ActivateGameover("Your ship was destroyed!");
                break;
            case "Player":
                UiManager.instance.ActivateGameover("Your health ran out!");
                break;
            case "Enemy":
                enemyStats = healthBar.gameObject.GetComponentInParent<EnemyStats>();
                enemyStats.KillEnemy();
                break;
        }
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        healthBar.UpdateHealth(health, maxHealth);
    }
}