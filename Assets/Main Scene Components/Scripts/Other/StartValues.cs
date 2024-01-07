using System.Collections;
using UnityEngine;
using UI;
using Enemy;
public class StartValues : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject enemyObj;
    public GameObject speedUpObj;
    public GameObject healthUpObj;
    public GameObject ammoUpObj;
    public GameObject powerupPosObj;

    [Header("Appear Rates")]
    private int powerupChargeAppear;
    private int enemyChargeAppear;

    private bool enemySpawned;
    private bool powerUpSpawned;

    private Vector3 enemyPos;
    private Charge charger;
        
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
        enemyPos = new Vector3(250, 0.6f, 250);

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
        float randomPos = Random.Range(245, 275);   // Adjust if having problems
        enemyPos = new Vector3(randomPos, 0.5f, randomPos);

        if (!enemySpawned && (int)charger.ChargeValue % enemyChargeAppear == 0)
        {
            enemySpawned = true;
            CreateEnemy();

            StartCoroutine(ResetObjectCreation("enemy"));
        }

        if(!powerUpSpawned && (int)charger.ChargeValue % powerupChargeAppear == 0)
        {
            powerUpSpawned = true;
            CreatePowerup();          

            StartCoroutine(ResetObjectCreation("powerup"));
        }

    }

    private void CreatePowerup()
    {
        int powerUpchosen = Random.Range(1, 4);

        Vector3 adjustedPowPos = new Vector3(powerupPosObj.transform.position.x, powerupPosObj.transform.position.y + 1f, powerupPosObj.transform.position.z);

        GameObject lol;
        switch (powerUpchosen)
        {
            case 1:
                lol = Instantiate(ammoUpObj, adjustedPowPos, Quaternion.Euler(270, 45, 45)); 
                lol.SetActive(true);
                break;
            case 2:
                lol = Instantiate(healthUpObj, adjustedPowPos, Quaternion.Euler(270, 45, 45));
                lol.SetActive(true);
                break;
            case 3:
                lol = Instantiate(speedUpObj, adjustedPowPos, Quaternion.Euler(270, 45, 45));
                lol.SetActive(true);
                break;
            default:
                lol = Instantiate(healthUpObj, adjustedPowPos, Quaternion.Euler(270, 45, 45)); // Put just to get rid of unassigned error but can changed
                lol.SetActive(true);
                break;
        }
    }

    private IEnumerator ResetObjectCreation(string specificObject) // Is this needed?
    {
        yield return new WaitForSeconds(1);
        if (specificObject == "powerup") { powerUpSpawned = false; }
        if (specificObject == "enemy") { enemySpawned = false; }
    }

    public void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(enemyObj, enemyPos, Quaternion.identity);
        newEnemy.name = "Enemy Clone";  // Why was this done? (don't remember)

        int stateChosen = Random.Range(1, 3);    // Change to 4 with patrol
        switch (stateChosen)
        {
            case 1:
                newEnemy.GetComponent<EnemyAI>().state = EnemyAI.State.CHASE;
                break;
            case 2:
                newEnemy.GetComponent<EnemyAI>().state = EnemyAI.State.SHIP;
                break;
        }
    }
}