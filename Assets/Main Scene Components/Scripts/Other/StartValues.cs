using System.Collections;
using UnityEngine;
using UI;
using Enemy;
public class StartValues : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject enemyObj;
    public GameObject playerObj;
    public GameObject timerObj;    // NAMES WAY TOO SIMILAR
    public GameObject speedUpObj;
    public GameObject healthUpObj;
    public GameObject ammoUpObj;
    public GameObject powerupPosObj;
    public int powerupChargeAppear;
    public int enemyChargeAppear;

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


    private bool isRunning;
    private bool enemySpawned;
    private bool powerUpSpawned;

    private Vector3 enemyPos;
    private Charge charger;

    private void Awake()
    {
        charger = timerObj.GetComponent<Charge>();
    }
    void Start()
    {
        enemyPos = new Vector3(250, 0.6f, 250);       

        isRunning = true;
        enemySpawned = false;
        powerUpSpawned = false;

        CreateEm();
        CreatePowerup();
    }

    void Update()
    {
        float randomPos = Random.Range(245, 275);   // Adjust if having problems
        enemyPos = new Vector3(randomPos, 0.5f, randomPos);

        if (!enemySpawned && (int)charger.ChargeValue % enemyChargeAppear == 0)
        {
            Debug.Log($"Charge value: {charger.ChargeValue}");
            CreateEm();

            enemySpawned=true;
            StartCoroutine(ResetEnemyCreation());
        }

        if(!powerUpSpawned && (int)charger.ChargeValue % powerupChargeAppear == 0)
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
                powerup = speedUpObj;
                break;
            case 2:
                powerup = healthUpObj;
                break;
            case 3:
                powerup = ammoUpObj;
                break;
            default: 
                powerup = healthUpObj; // Put just to get rid of unassigned error but can changed
                break;
        }
        Vector3 adjustedPowPos = new Vector3(powerupPosObj.transform.position.x, powerupPosObj.transform.position.y + 1f, powerupPosObj.transform.position.z);

        Instantiate(powerup, adjustedPowPos, Quaternion.Euler(270, 45, 45));
        //GameObject newPowerup = Instantiate(powerup, powerupPos.transform.position, Quaternion.Euler(270,45,45));
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

    public void CreateEm()
    {
        GameObject newEnemy = Instantiate(enemyObj, enemyPos, Quaternion.identity);
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
