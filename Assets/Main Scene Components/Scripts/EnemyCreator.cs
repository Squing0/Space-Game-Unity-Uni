using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public GameObject enemy;
    public Transform createPosition;
    private bool isRunning;
    Transform pos;


    void Start()
    {
        pos = transform;
        pos.position = new Vector3 (0, 0, 0);

        isRunning = true;
        StartCoroutine(CreateEnemy());
    }

    void Update()
    {
        //if (!isRunning) { 
        //    StartCoroutine(CreateEnemy());
        //}
    }

    public IEnumerator CreateEnemy() { 
        isRunning = true;

        //Vector3 poss = enemy.transform.position;
        //poss.y += 2;

        Instantiate(enemy, createPosition.position, Quaternion.identity);

        yield return new WaitForSeconds(15f);

        isRunning = false;
    }
}
