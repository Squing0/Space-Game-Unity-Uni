using Assets.Main_Scene_Components.Scripts.Other;
using Enemy;
using UI;

// Central health class to manage health and sliders.
public class HealthManager : HealthMethods  // Interface is used as base class as mono behaviours can't be instantiated.
{
    // Health variables
    public int health;
    private int maxHealth;

    private SliderManager healthBar;
    private string type;    // Type of gameobject using class.

    // Needed for enemy type specifically
    private EnemyStats enemyStats;

    // Constructor to set health variables and specific type used 
    public HealthManager(int hea, int maxHea, SliderManager heaBar, string type)
    {
        health = hea;
        maxHealth = maxHea;
        healthBar = heaBar;
        this.type = type;
    }
    // Health is decreased and slider is updated. 
    public void DecreaseHealth(int damage)
    {
        health -= damage;
        healthBar.UpdateHealth(health, maxHealth);

        if(health < 1)  // If health runs out, then game is ended.
        {
            gameEnd(type);
        }
    }

    // Ends game in different ways depending on types.
    private void gameEnd(string type)
    {
        switch (type)
        {
            case "Ship":    // Game over screen is shown for ship and player with different messages.
                UiManager.instance.ActivateGameover("Your ship was destroyed!");
                break;
            case "Player":
                UiManager.instance.ActivateGameover("Your health ran out!");
                break;
            case "Enemy":   // Enemy is removed from scene and game over screen is not shown.
                enemyStats = healthBar.gameObject.GetComponentInParent<EnemyStats>();
                enemyStats.KillEnemy();
                break;
        }
    }
    // Health is increased and slider is updated. 
    public void IncreaseHealth(int amount)
    {
        health += amount;
        healthBar.UpdateHealth(health, maxHealth);
    }
}