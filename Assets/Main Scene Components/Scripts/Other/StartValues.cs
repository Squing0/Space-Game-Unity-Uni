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
    public GameObject playerObj;
    public GameObject timerObj;    // NAMES WAY TOO SIMILAR
    public GameObject powerupPosObj;

    [Header("Appear Rates")]
    public int powerupChargeAppear;
    public int enemyChargeAppear;

    private bool enemySpawned;
    private bool powerUpSpawned;

    private Vector3 enemyPos;
    private Charge charger;
    private GameObject createdPowerup = null;

    //public GameObject CreatedPowerup  
    //{
    //    get { return CreatedPowerup; } 
    //}
        
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
        charger = timerObj.GetComponent<Charge>();
    }
    void Start()
    {
        enemyPos = new Vector3(250, 0.6f, 250);       

        enemySpawned = false;
        powerUpSpawned = false;

        CreateEnemy();
        CreatePowerup();
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
        //GameObject powerup;
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

        //powerup.name = "Powerup Clone";
        // GameObject createdpowerup =  Instantiate(powerup, adjustedPowPos, Quaternion.Euler(270, 45, 45));   // Assigning the powerup to the prefab destroys it, so it can't be accessed in the scene
        //Instantiate(createdPowerup, adjustedPowPos, Quaternion.Euler(270, 45, 45));
        //powerup.SetActive(true);
        //GameObject newPowerup = Instantiate(powerup, powerupPos.transform.position, Quaternion.Euler(270,45,45));
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
                newEnemy.GetComponent<BasicAi>().state = BasicAi.State.CHASE;
                break;
            case 2:
                newEnemy.GetComponent<BasicAi>().state = BasicAi.State.SHIP;
                break;
        }
    }
}