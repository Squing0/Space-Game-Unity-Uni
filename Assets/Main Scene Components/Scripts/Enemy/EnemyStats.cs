using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UI;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        [Header("Health")]
        public int health = 3;
        public int maxHealth = 3;
        public HealthBar healthBar;    // Change this to be more efficient

        [Header("Enemy")]
        public NavMeshAgent agent;
        public float enemyAliveTime;
        public GameObject enemyPrefab;

        public GameObject Charger;
        private Charge charge;

        public int Health
        {
            get { return health; }
            set { health = value; }  // Use MAX health here too
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        private void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            agent = GetComponent<NavMeshAgent>();   // Do this like this for other scripts, is way Peter does it.
        }

        private void Start()
        {
            charge = Charger.GetComponent<Charge>();
            StartCoroutine(DestroyEnemy());
        }
        private void Update()
        {
            if (health < 1)
            {
                agent.isStopped = true;
                Destroy(gameObject);
            }
        }

        public void reduceHealth(int damage)
        {
            health -= damage;
            healthBar.updateHealth(health, maxHealth);
        }

        public void IncreaseHealth(int amount)
        {
            health += amount;
            healthBar.updateHealth(health, maxHealth);
        }

        public IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(enemyAliveTime / charge.ChargeSpeeder);   // Might not change much, but more to make consistent with powerups

            if(gameObject.name != enemyPrefab.name) // Try to change this if possible
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                reduceHealth(1);
            }
        }       
    }
}