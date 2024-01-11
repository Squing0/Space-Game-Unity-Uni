using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UI;

namespace Enemy
{
    // Handles all variables and methods related to stats of enemy object.
    public class EnemyStats : MonoBehaviour
    {
        [Header("Health")]
        public SliderManager healthBar;    
        public HealthManager healthManager;
        public int maxHealth;

        [Header("Enemy")]
        public NavMeshAgent agent;
        public float enemyAliveTime;
        public string enemyPrefabName;
        public string deathAnimation;
        public GameObject enemyHealthSlider;

        [Header("Other")]
        public Camera mCamera;
        public GameObject moralityBarObj;

        // Scripts to manipulate.
        private Charge charge;
        private EnemyAI ai;
        private SliderManager moralityBar;
        private Animator animator;
        
        private int health;    

        // Properties used as variables accessed in other scripts.
        public int Health
        { get { return health;}}

        public int MaxHealth
        { get { return maxHealth; } set { maxHealth = value;}
        }

        private void Awake()
        {
            // Scripts gotten in awake as all on enemy.
            healthBar = GetComponentInChildren<SliderManager>();
            agent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();
            moralityBar = moralityBarObj.GetComponent<SliderManager>();
            animator = GetComponent<Animator>();

            health = maxHealth;     // Current health set to full.
            mCamera = Camera.main;  // Set to only camera in scene.
        }

        private void Start()
        {
            charge = FindAnyObjectByType<Charge>(); // Charge found in start as not on enemy.

            // Health manager instantiated to handle all health methods.
            healthManager = new HealthManager(health, maxHealth, healthBar, "Enemy");

            // Enemy set on timer to be destroyed when object instantiated. 
            StartCoroutine(DestroyEnemy());
        }
        private void Update()
        {
            // enemy health slider always facing player.
            enemyHealthSlider.transform.rotation = mCamera.transform.rotation;
        }

        // Stops enemy and plays audio and death animations before killing.
        public void KillEnemy()
        {
            agent.isStopped = true;

            AudioManager.instance.enemyDeathSound.Play();
            animator.Play(deathAnimation);

            StartCoroutine(WaitKillEnemy());   
        }

        // Coroutine is used so animation can be seen and sound can be heard.
        private IEnumerator WaitKillEnemy()
        {
            yield return new WaitForSeconds(1f);

            CheckMorality();    // Morality will be changed depending on enemy state.
            Destroy(gameObject);
        }

        // Destroys enemy object if not killed by player.
        private IEnumerator DestroyEnemy()
        {
            // Charge speeder used to change time depending on start values.
            yield return new WaitForSeconds(enemyAliveTime / charge.ChargeSpeeder);  

            // Name specifically checked against alien prefab so prefab not deleted.
            if(gameObject.name != enemyPrefabName) 
            {
                Destroy(gameObject);
            }   
        }

        private void OnTriggerEnter(Collider other)
        {
            // Only bullet decreases enemy health.
            if (other.gameObject.CompareTag("Bullet"))
            {
                healthManager.DecreaseHealth(1);
            }
        }     
        
        // Increases or decreases morality bar depending on player state.
        private void CheckMorality()
        {
            if (ai.state == EnemyAI.State.PATROL)    // Only increases if state is patrol.
            {
                moralityBar.ChangeMorality(.1f);
            }
            else    // Decreases if not.
            {
                moralityBar.ChangeMorality(-.1f);
            }
        }
    }
}