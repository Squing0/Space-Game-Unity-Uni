using Enemy;
using Player;
using UI;
using UnityEngine;

// Changes game values based on difficulty selected.
public class Difficulty : MonoBehaviour
{
    public string difficulty;

    public GameObject enemyObj;

    // Scripts to get values from.
    private Charge charger;    
    private StartValues enemyCreator;   
    private PlayerMovement player;
    private EnemyAI enemyAi;

    // Values to change.
    private float time = 1;       
    private int powerupTime = 1;
    private int enemyTime = 1;
    private int maxHealth = 1;
    private int health = 1;
    private float walkSpeed = 1;
    private float runSpeed = 1;
    private float enemyAttackTime = 1;

    private void Awake()
    {
        difficulty = DifficultyAcrossScenes.instance.difficulty;    // Difficuly gotten in awake specifically as instance is used here.

    }
    private void Start()
    {
        // Scripts are found.
        charger = FindAnyObjectByType<Charge>();    
        enemyCreator = FindAnyObjectByType<StartValues>();
        player = FindAnyObjectByType<PlayerMovement>();
        enemyAi = enemyObj.GetComponent<EnemyAI>();

        SelectDifficulty();
    }

    // Different values are applied based on selected difficulty.
    private void SelectDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                SetValues(2, 5, 15, 10, 10, 10, 15, 3f);
                break;
            case "Normal":
                SetValues(1, 10, 10, 8, 8, 9, 14, 2.5f);
                break;
            case "Hard":
                SetValues(1, 15, 8, 6, 6, 8, 13, 2f);
                break;
        }

        charger.ChargeSpeeder = time;
        enemyCreator.PowerupChargeAppear = powerupTime;
        enemyCreator.EnemyChargeAppear = enemyTime;
        player.MaxHealth = maxHealth;
        player.Health = health;
        player.WalkSpeed = walkSpeed;
        player.RunSpeed = runSpeed;
        enemyAi.timeBetweenAttacks = enemyAttackTime;
    }

    // Values are set depending on what parameters given.
    public void SetValues(float timeV, int pTimeV, int eTimeV, int healthV, int maxHealthV, float wSpeedV, float rSpeedV, float eAttackTimeV)
    {
        time = timeV;
        powerupTime = pTimeV;
        enemyTime = eTimeV;
        maxHealth = maxHealthV;
        health = healthV;
        walkSpeed = wSpeedV;
        runSpeed = rSpeedV;
        enemyAttackTime = eAttackTimeV;
    }
}
