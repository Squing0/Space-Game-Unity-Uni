using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public GameObject enemy;
    public GameObject ship;
    public GameObject player;

    private bool isRunning;
    private Vector3 enemyPos;


    void Start()
    {
        enemyPos = new Vector3 (250, 0.5f, 250);

        //BasicAi enemyAi = enemy.GetComponent<BasicAi>();
        //enemyAi.ship = ship;
        //enemyAi.target = player;

        //EnemyHealthBar health = enemy.GetComponentInChildren<EnemyHealthBar>();
        //health.camera = Camera.main;
        

        isRunning = true;   
        StartCoroutine(CreateEnemy());
    }

    void Update()
    {
        if (!isRunning)
        {
            StartCoroutine(CreateEnemy());
        }
    }

    public IEnumerator CreateEnemy() { 
        isRunning = true;

        //Vector3 poss = enemy.transform.position;
        //poss.y += 2
        
        GameObject newEnemy = Instantiate(enemy, enemyPos, Quaternion.identity);
        newEnemy.name = "Enemy Clone";

        //BasicAi newEnemyAi = newEnemy.GetComponent<BasicAi>();
        //newEnemyAi.ship = ship;
        //newEnemyAi.target = player;

        //newEnemy.AddComponent   <BasicAi>();
        //EnemyHealthBar newHealth = newEnemy.GetComponentInChildren<EnemyHealthBar>();

        yield return new WaitForSeconds(15f);

        isRunning = false;
    }
}
