using Enemy;
using Player;
using UI;
using System.Threading;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public string difficulty;
    public GameObject timerObj;
    public GameObject enemyCreatorObj;
    public GameObject playerObj;
    public GameObject enemyObj;

    private UI.Timer timer;    //Make how access variables here consistent (if annoying get rid of properties
    private EnemyCreator enemyCreator;
    private PlayerMovement player;
    private BasicAi enemyAi;

    private void Start()
    {
        difficulty = DifficultyAcrossScenes.instance.difficulty;

        timer = timerObj.GetComponent<UI.Timer>();
        enemyCreator = enemyCreatorObj.GetComponent<EnemyCreator>();
        player = playerObj.GetComponent<PlayerMovement>();
        enemyAi = enemyObj.GetComponent<BasicAi>();

        SelectDifficulty();
    }

    private void SelectDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                DifficultyChoice("Easy");
                break;
            case "Normal":
                DifficultyChoice("Normal");
                break;
            case "Hard":
                DifficultyChoice("Hard");
                break;
        }
    }


    public void DifficultyChoice(string difficulty)
    {

        float time = 1;
        int powerupTime = 1;
        int enemyTime = 1;
        int maxHealth = 1;  // NEED TO INCLUDE BASE HEALTH HERE TOO
        int health = 1;
        float walkSpeed = 1; // Change so that only need to alter one speed value?
        float runSpeed = 1;
        float enemyAttackTime = 1;

        switch(difficulty)  // Have better way of doing this, possibly with method?
        {
            case "Easy":
                time = 2;
                powerupTime = 5;
                enemyTime = 15;
                maxHealth = 10;
                health = 10;    // hate wat of doing this but player health only gets set to max in start of player movement script.
                walkSpeed = 10;
                runSpeed = 15;
                enemyAttackTime = 3;
                break;
            case "Normal":
                time = 1f;
                powerupTime = 10;
                enemyTime = 10;
                maxHealth = 8;
                health = 8;
                walkSpeed = 9;
                runSpeed = 14;
                enemyAttackTime = 2.5f;
                break;
            case "Hard":
                time = 1f;
                powerupTime = 15;
                enemyTime = 8;
                maxHealth = 6;
                health = 6;
                walkSpeed = 8;
                runSpeed = 13;
                enemyAttackTime = 2;
                break;
        }

        timer.TimeSpeeder = time;   // DON'T FORGET NAMESPACES
        enemyCreator.PowerupChargeAppear = powerupTime;
        enemyCreator.EnemyChargeAppear = enemyTime;
        player.MaxHealth = maxHealth;
        player.Health = health;
        player.walkSpeed = walkSpeed; // Change so that only need to alter one speed value?
        player.runSpeed = runSpeed;
        enemyAi.timeBetweenAttacks = enemyAttackTime;
    }
}
