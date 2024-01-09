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
        public HealthBar healthBar;    // Change this to be more efficient

        [Header("Enemy")]
        public NavMeshAgent agent;
        public float enemyAliveTime;
        public string enemyPrefabName;
        public string deathAnimation;

        private Charge charge;
        private EnemyAI ai;
        private HealthBar moralityBar;
        private Animator animator;
        public GameObject moralityBarObj;

        public int Health
        { get { return health;}}

        public int MaxHealth
        { get { return maxHealth; } set { maxHealth = value;}
        }

        private void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            agent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();
            moralityBar = moralityBarObj.GetComponent<HealthBar>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            charge = FindAnyObjectByType<Charge>();

            StartCoroutine(DestroyEnemy());

            health = maxHealth;
            //healthBar = new HealthBar(health, maxHealth);
        }
        private void Update()
        {
            if (health < 1)
            {
                agent.isStopped = true;

                AudioManager.instance.enemyDeathSound.Play();
                animator.Play(deathAnimation);
                
                StartCoroutine(PlayDeathAnimation());   // Change name

            }
        }

        private IEnumerator PlayDeathAnimation()
        {
            yield return new WaitForSeconds(1f);
            CheckMorality();
            Destroy(gameObject);

        }

        public void DecreaseHealth(int damage)
        {
            health -= damage;
            healthBar.UpdateHealth(health, maxHealth);
        }

        public void IncreaseHealth(int amount)
        {
            health += amount;
            healthBar.UpdateHealth(health, maxHealth);
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(enemyAliveTime / charge.ChargeSpeeder);  // Might not change much, but more to make consistent with powerups

            if(gameObject.name != enemyPrefabName) // Try to change this if possible
            {
                //CheckMorality();  // needed here? 
                Destroy(gameObject);
            }   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                DecreaseHealth(1);
            }
        }     
        
        private void CheckMorality()
        {
            if (ai.state == EnemyAI.State.PATROL)
            {
                moralityBar.IncreaseMorality(.1f);
            }
            else if (ai.state != EnemyAI.State.PATROL)
            {
                moralityBar.DecreaseMorality(.1f);
            }
        }
    }
}