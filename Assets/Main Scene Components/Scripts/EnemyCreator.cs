using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public GameObject enemy;
    public GameObject ship;
    public GameObject player;
    public GameObject timer;    // NAMES WAY TOO SIMILAR
    public GameObject speedUp;
    public GameObject healthUp;
    public GameObject ammoUp;
    
    private bool isRunning;
    private bool enemySpawned;
    private bool powerUpSpawned;

    private Vector3 enemyPos;
    private Timer time;

    private void Awake()
    {
        time = timer.GetComponent<Timer>();
    }
    void Start()
    {
        enemyPos = new Vector3(250, 0.5f, 250);

        //BasicAi enemyAi = enemy.GetComponent<BasicAi>();
        //enemyAi.ship = ship;
        //enemyAi.target = player;

        //EnemyHealthBar health = enemy.GetComponentInChildren<EnemyHealthBar>();
        //health.camera = Camera.main;


        isRunning = true;
        enemySpawned = false;
        powerUpSpawned = false;
        CreateEm();
        CreatePowerup();
    }

    void Update()
    {
        float randomPos = Random.Range(245, 255);
        enemyPos = new Vector3(randomPos, 0.5f, randomPos);

        //if (!isRunning)
        //{
        //    StartCoroutine(CreateEnemy());
        //}

        //if (Mathf.Abs(time.Charge - Mathf.Round(time.Charge / 15f) * 15f) < 0.0001f)
        //{
        //    CreateEm();
        //}

        //if (time.Charge % 15f == 0.002f)
        //{
        //    CreateEm();
        //}

        if (!enemySpawned && (int)time.Charge % 15 == 0)
        {
            Debug.Log($"Charge value: {time.Charge}");
            CreateEm();

            enemySpawned=true;
            StartCoroutine(ResetEnemyCreation());
        }

        if(!powerUpSpawned && (int)time.Charge % 10 == 0)
        {
            CreatePowerup();
            powerUpSpawned = true;
            StartCoroutine(ResetPowerUpCreation());
        }

    }

    private void CreatePowerup()
    {
        int powerUpchosen = Random.Range(1, 4);
        GameObject powerup;
        switch (powerUpchosen)
        {
            case 1:
                powerup = speedUp;
                break;
            case 2:
                powerup = healthUp;
                break;
            case 3:
                powerup = ammoUp;
                break;
            default: 
                powerup = healthUp; // Put just to get rid of unassigned error but can changed
                break;
        }
        GameObject newPowerup = Instantiate(powerup, enemyPos, Quaternion.identity);
    }

    private IEnumerator ResetEnemyCreation()
    {
        yield return new WaitForSeconds(1);
        enemySpawned = false;
    }
    private IEnumerator ResetPowerUpCreation()
    {
        yield return new WaitForSeconds(1);
        powerUpSpawned = false; // EXACT SAME AS ABOVE?
    }
    //public IEnumerator CreateEnemy() {
    //    isRunning = true;

    //    //Vector3 poss = enemy.transform.position;
    //    //poss.y += 2

    //    GameObject newEnemy = Instantiate(enemy, enemyPos, Quaternion.identity);
    //    newEnemy.name = "Enemy Clone";

    //    //BasicAi newEnemyAi = newEnemy.GetComponent<BasicAi>();
    //    //newEnemyAi.ship = ship;
    //    //newEnemyAi.target = player;

    //    //newEnemy.AddComponent   <BasicAi>();
    //    //EnemyHealthBar newHealth = newEnemy.GetComponentInChildren<EnemyHealthBar>();

    //    yield return new WaitForSeconds(15f);

    //    isRunning = false;
    //}

    public void CreateEm()
    {
        GameObject newEnemy = Instantiate(enemy, enemyPos, Quaternion.identity);
        newEnemy.name = "Enemy Clone";

        BasicAi AI = newEnemy.GetComponent<BasicAi>();

        int stateChosen = Random.Range(1, 3);    // Change to 4 with patrol
        switch (stateChosen)
        {
            case 1:
                newEnemy.GetComponent<BasicAi>().state = BasicAi.State.CHASE;
                break;
            case 2:
                newEnemy.GetComponent<BasicAi>().state = BasicAi.State.SHIP;
                break;
        }
        Debug.Log(AI.state);

    }
}
