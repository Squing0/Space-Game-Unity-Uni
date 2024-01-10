using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public NavMeshAgent agent;

        public enum State { PATROL, CHASE, SHIP, ATTACK }    
        public State state;

        public GameObject ship;

        public float patrolSpeed = 0.5f;
        public float chaseSpeed = 1.0f;
        public float attackSpeed = 1.1f;
        public GameObject playerObj;
        private GunShoot gunShoot;

        public LayerMask isPlayer;
        public float attackRange;
        public bool PlayerWithinRange;
        public bool alreadyAttacked;

        // patrol speed
        private Vector3 newPatrolPos;
        private bool patrolPointFound;
        public float patrolRange;

        public GameObject rockObj;
        private ProjectileManager projectileManager;
        private Rigidbody rb;

        public GameObject knifeObj;
        public float timeBetweenAttacks;

        // time attack property
        //public float TimeBetweenAttacks // Doesn't work for some reason
        //{ get { return timeBetweenAttacks; } set { timeBetweenAttacks = value; }}

        [Header("Animations")]
        public Animator enemyAnimator;
        public string runAnimation;
        public string walkAnimation;
        public string combatRunAnimation;
        public string attackAnimation;

        Vector3 playerPos;
        public LayerMask whatisGround;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.updatePosition = true;
            agent.updateRotation = true;

            patrolPointFound = true;
        }
        private void Start()
        {
            projectileManager = rockObj.GetComponent<ProjectileManager>();
            gunShoot = FindAnyObjectByType<GunShoot>();

            StartCoroutine(FSM());
        }

        IEnumerator FSM()
        {
            while (true)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                    case State.SHIP:
                        Ship();
                        break;
                    case State.ATTACK:
                        AttackPlayer();
                        break;
                }

                yield return null;
            }
        }

        private void Update()
        {
            PlayerWithinRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
            bool playerTooClose = Physics.CheckSphere(transform.position, 5, isPlayer);

            if (PlayerWithinRange && state == State.CHASE)  // So that doesn't inerfere with ship or patrol
            {
                state = State.ATTACK;
            }
            else if (!PlayerWithinRange && state == State.ATTACK)
            {
                state = State.CHASE;
            }

            if(PlayerWithinRange && state == State.SHIP && gunShoot.CurrentBullets == 0 && gunShoot.bulletTotal == 0)
            {
                state = State.CHASE;
            }

            if(playerTooClose && state == State.ATTACK)
            {
                agent.speed = 0;
            }
            else
            {
                agent.speed = chaseSpeed;
            }
        }

        private void AttackPlayer()
        {
           
            transform.LookAt(playerObj.transform.position);
            agent.SetDestination(playerObj.transform.position);

            agent.updateRotation = false;

            if (!alreadyAttacked)
            {
                rb = Instantiate(rockObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

                rb.velocity = transform.forward * 30;
                StartCoroutine(projectileManager.deleteProjectile(rb.gameObject));

                alreadyAttacked = true;
                enemyAnimator.Play(attackAnimation);

                StartCoroutine(ResetAttacked());  // For some reason doesn't work
                //Invoke(nameof(ResetAttack), timeBetweenAttacks);    // CHANGE TO COROUTINE
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private IEnumerator ResetAttacked()
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            alreadyAttacked = false;       
        }
     
        private void Ship()
        {
            knifeObj.SetActive(true);   // Should only have to be in one place
            agent.SetDestination(ship.transform.position);

            if (!alreadyAttacked)   // Just copied from attack method
            {
                enemyAnimator.Play(attackAnimation);
                alreadyAttacked = true;

                //StartCoroutine(ResetAttacked());
                Invoke(nameof(ResetAttack), 2f);
            }
        }
        private void Chase()
        {
            agent.speed = chaseSpeed;
            transform.LookAt(playerObj.transform.position);
            agent.SetDestination(playerObj.transform.position);

            knifeObj.SetActive(false);

            enemyAnimator.StopPlayback();
            enemyAnimator.Play(runAnimation);
        }

        private void Patrol()
        {
            agent.speed = patrolSpeed;
            enemyAnimator.Play(walkAnimation);

            if(patrolPointFound)
            {
                newPatrolPos = FindNewPatrolPoint();
                agent.SetDestination(newPatrolPos);

                patrolPointFound = false;
                StartCoroutine(ResetpatrolPos());
            }
        }

        private IEnumerator ResetpatrolPos()
        {
            yield return new WaitForSeconds(3); // Hard coded, change
            patrolPointFound = true;
        }

        private Vector3 FindNewPatrolPoint()    // Be careful, got this method from youtube tutorial
        {
            float randomx = Random.Range(-patrolRange, patrolRange);
            float randomz = Random.Range(-patrolRange, patrolRange);

            return new Vector3(gameObject.transform.position.x + randomx, 
                0, 
                gameObject.transform.position.z + randomz);           
        }
     
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public void SpeedUpActivate(float speedIncrease, float speedTime)   // Copied from player movement (use interface?)
        {
            agent.speed += speedIncrease;
            StartCoroutine(SpeedTimer(speedIncrease, speedTime));
        }

        private IEnumerator SpeedTimer(float speedDecrease, float speedCooldown)
        {
            yield return new WaitForSeconds(speedCooldown);
            agent.speed -= speedDecrease;
        }
    }
}