using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public GameObject enemy;
    public GameObject ship;
    public GameObject player;
    public GameObject timer;    // NAMES WAY TOO SIMILAR
    
    private bool isRunning;
    private bool enemySpawned;
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
        CreateEm();
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

    }

    private IEnumerator ResetEnemyCreation()
    {
        yield return new WaitForSeconds(1);
        enemySpawned = false;
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
    }
}
