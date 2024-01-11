using System.Collections;
using UnityEngine;
using UI;
using Enemy;
// Handles instantiation of enemies and powerups at start of game.
public class StartValues : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject enemyObj;
    public GameObject speedUpObj;
    public GameObject healthUpObj;
    public GameObject ammoUpObj;

    [Header("Powerup Position")]
    public Transform powerupPos;

    private int powerupChargeAppear;    // Specific charge time that powerups and enemies appear at.
    private int enemyChargeAppear;

    // Whether enemies or powerups have spawned.
    private bool enemySpawned;
    private bool powerUpSpawned;

    private Vector3 enemyPos;   // Specific position of enemy
    private Charge charger;
        
    // Properties used as variables accessed in other methods.
    public int PowerupChargeAppear
    {
        get { return powerupChargeAppear; }
        set { powerupChargeAppear = value; }
    }
    public int EnemyChargeAppear
    {
        get { return enemyChargeAppear; }
        set {  enemyChargeAppear = value; }
    }

    private void Awake()
    {
        enemyPos = new Vector3(250, 0.6f, 250); // Ensures that object is instantiated on navmesh.

        // Powerups and enemies initially not spawned.
        enemySpawned = false;
        powerUpSpawned = false;
    }
    void Start()
    {
        charger = FindAnyObjectByType<Charge>();

        CreateEnemy();
        CreatePowerup();

        Time.timeScale = 1; // Resumes game if paused from earlier playthrough
    }

    void Update()
    {
        float randomPos = Random.Range(245, 275);   // x and y positions are randomly generated and applied to existing enemy object.
        enemyPos = new Vector3(randomPos, 0.5f, randomPos);

        if (!enemySpawned && (int)charger.ChargeValue % enemyChargeAppear == 0) // Modulo used to instantiate enemy at specific point in charge.
        {
            enemySpawned = true;
            CreateEnemy();

            StartCoroutine(ResetObjectCreation("enemy"));   // Enemy creation reset ensures only one enemy created.
        }

        if (!powerUpSpawned && (int)charger.ChargeValue % powerupChargeAppear == 0) // Modulo used to instantiate powerup at specific point in charge.
        {
            powerUpSpawned = true;
            CreatePowerup();

            StartCoroutine(ResetObjectCreation("powerup")); // Powerup creation reset ensures only one powerup created.
        }

    }

    // Randomly assigned one powerup of three to create.
    private void CreatePowerup()
    {
        int powerUpchosen = Random.Range(1, 4); 

        switch (powerUpchosen)
        {
            case 1:
                CreateSpecificPowerup(ammoUpObj);
                break;
            case 2:
                CreateSpecificPowerup(healthUpObj);
                break;
            case 3:
                CreateSpecificPowerup(speedUpObj);
                break;
        }
    }
    // Powerup is instantiated at specific rotation.
    private void CreateSpecificPowerup(GameObject powerupChosen)
    {
        GameObject powerup;
     
        powerup = Instantiate(powerupChosen, powerupPos.position, Quaternion.Euler(270, 45, 45));
        powerup.SetActive(true);    // Set to active as based off of unactive powerup prefab.
    }

    // Waits for one second before spawning other enemy or powerup.
    private IEnumerator ResetObjectCreation(string specificObject) 
    {
        yield return new WaitForSeconds(1); 
        if (specificObject == "powerup") { powerUpSpawned = false; }
        if (specificObject == "enemy") { enemySpawned = false; }
    }

    // Creates enemy with randomly chosen state of three.
    public void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(enemyObj, enemyPos, Quaternion.identity);
        newEnemy.name = "Enemy Clone";  // Enemy name changed to be different than initial prefab.

        int stateChosen = Random.Range(1, 4);    
        switch (stateChosen)
        {
            case 1:
                newEnemy.GetComponent<EnemyAI>().state = EnemyAI.State.CHASE;
                break;
            case 2:
                newEnemy.GetComponent<EnemyAI>().state = EnemyAI.State.SHIP;
                break;
            case 3:
                newEnemy.GetComponent<EnemyAI>().state = EnemyAI.State.PATROL;
                break;
        }
    }
}