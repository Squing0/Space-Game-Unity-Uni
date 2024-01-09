using Enemy;
using Unity.VisualScripting;
using UnityEngine;

public class Morality : MonoBehaviour
{
    public GameObject enemyObj;
    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = enemyObj.GetComponent<EnemyAI>();
    }

    private void Update()
    {
        //if(enemyAI.state == EnemyAI.State.PATROL && enemyObj.IsDestroyed()) 
        //{
        //    Debug.Log("+1");
        //}
        //else if (enemyObj.IsDestroyed())
        //{
        //    Debug.Log("-1");
        //}
    }
}
