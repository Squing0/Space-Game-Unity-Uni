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

        public NavMeshAgent agent;
        public HealthBar healthBar;    // Change this to be more efficient
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

        public IEnumerator DestroyEnemy()
        {

            yield return new WaitForSeconds(40f);   // Make variable?

            if (gameObject.name != "Zolrik (1)")
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb;
            rb = other.gameObject.GetComponent<Rigidbody>();

            if (other.gameObject.CompareTag("Bullet"))
            {
                reduceHealth(1);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.tag == "Player") Can use if needed
            //{
            //    state = State.CHASE;
            //    target = collision.gameObject;
            //}
        }
    }
}