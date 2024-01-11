using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UI;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        [Header("Health")]
        private int health;
        public int maxHealth;   // Could set to private but means will only work with difficulty selected.
        public SliderManager healthBar;    // Change this to be more efficient
        public HealthManager healthManager;

        [Header("Enemy")]
        public NavMeshAgent agent;
        public float enemyAliveTime;
        public string enemyPrefabName;
        public string deathAnimation;
        public Camera mCamera;
        public GameObject enemyHealthSlider;

        private Charge charge;
        private EnemyAI ai;
        private SliderManager moralityBar;
        private Animator animator;
        public GameObject moralityBarObj;

        public int Health
        { get { return health;}}

        public int MaxHealth
        { get { return maxHealth; } set { maxHealth = value;}
        }

        private void Awake()
        {
            healthBar = GetComponentInChildren<SliderManager>();
            agent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();
            moralityBar = moralityBarObj.GetComponent<SliderManager>();
            animator = GetComponent<Animator>();

            health = maxHealth;
            mCamera = Camera.main;
        }

        private void Start()
        {
            charge = FindAnyObjectByType<Charge>();

            StartCoroutine(DestroyEnemy());

            healthManager = new HealthManager(health, maxHealth, healthBar, "Enemy");
        }
        private void Update()
        {
            enemyHealthSlider.transform.rotation = mCamera.transform.rotation;

            if (health < 1)
            {
                agent.isStopped = true;

                AudioManager.instance.enemyDeathSound.Play();
                animator.Play(deathAnimation);
                
                StartCoroutine(PlayDeathAnimation());   // Change name

            }
        }

        public void KillEnemy()
        {
            agent.isStopped = true;

            AudioManager.instance.enemyDeathSound.Play();
            animator.Play(deathAnimation);

            StartCoroutine(PlayDeathAnimation());   // Change name
        }

        private IEnumerator PlayDeathAnimation()
        {
            yield return new WaitForSeconds(1f);
            CheckMorality();
            Destroy(gameObject);

        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(enemyAliveTime / charge.ChargeSpeeder);  

            if(gameObject.name != enemyPrefabName) 
            {
                Destroy(gameObject);
            }   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                healthManager.DecreaseHealth(1);
            }
        }     
        
        private void CheckMorality()
        {
            if (ai.state == EnemyAI.State.PATROL)
            {
                moralityBar.ChangeMorality(.1f);
            }
            else if (ai.state != EnemyAI.State.PATROL)
            {
                moralityBar.ChangeMorality(-.1f);
            }
        }
    }
}