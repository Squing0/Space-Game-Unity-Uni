using Enemy;
using Player;
using System.Data;
using UI;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    [Header("Difficulty")]
    public string difficulty;

    [Header("Game Objects")]
    public GameObject timerObj;
    public GameObject enemyCreatorObj;
    public GameObject playerObj;
    public GameObject enemyObj;

    private Charge charger;    //Make how access variables here consistent (if annoying get rid of properties)
    private StartValues enemyCreator;   // CHANGE THIS
    private PlayerMovement player;
    private EnemyAI enemyAi;

    private float time = 1;       
    private int powerupTime = 1;
    private int enemyTime = 1;
    private int maxHealth = 1;
    private int health = 1;
    private float walkSpeed = 1;
    private float runSpeed = 1;
    private float enemyAttackTime = 1;

    private void Start()
    {
        difficulty = DifficultyAcrossScenes.instance.difficulty;    // (This flags as an error but functions perfectly)

        charger = timerObj.GetComponent<Charge>();
        enemyCreator = enemyCreatorObj.GetComponent<StartValues>();
        player = playerObj.GetComponent<PlayerMovement>();
        enemyAi = enemyObj.GetComponent<EnemyAI>();

        SelectDifficulty();
    }

    private void SelectDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                SetValues(2, 5, 15, 10, 10, 10, 15, 3);
                break;
            case "Normal":
                SetValues(1, 10, 10, 8, 8, 9, 14, 2.5f);

                break;
            case "Hard":
                SetValues(1, 15, 8, 6, 6, 8, 13, 2);
                break;
        }

        charger.ChargeSpeeder = time;
        enemyCreator.PowerupChargeAppear = powerupTime;
        enemyCreator.EnemyChargeAppear = enemyTime;
        player.MaxHealth = maxHealth;
        player.Health = health;
        player.walkSpeed = walkSpeed;
        player.runSpeed = runSpeed;
        enemyAi.timeBetweenAttacks = enemyAttackTime;
    }

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
