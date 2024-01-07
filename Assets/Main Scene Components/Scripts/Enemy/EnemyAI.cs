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
        public GameObject player;

        public LayerMask isPlayer;
        public float attackRange;
        public bool PlayerWithinRange;
        public bool alreadyAttacked;

        public GameObject rockObj;
        private ProjectileManager projectileManager;
        private Rigidbody rb;

        public GameObject knifeObj;
        public float timeBetweenAttacks;

        [Header("Animations")]
        public Animator enemyAnimator;
        public string runAnimation; 
        public string combatRunAnimation;
        public string attackAnimation;

        Vector3 playerPos;
        public LayerMask whatisGround;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.updatePosition = true;
            agent.updateRotation = true;
        }
        private void Start()
        {
            projectileManager = rockObj.GetComponent<ProjectileManager>();

            StartCoroutine(FSM());
        }

        IEnumerator FSM()
        {
            while (true)
            {
                switch (state)
                {
                    //case State.PATROL:
                    //    patrol();
                    //    break;
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

            if (PlayerWithinRange && state == State.CHASE)  // So that doesn't inerfere with ship or patrol
            {
                state = State.ATTACK;
            }
            else if (!PlayerWithinRange && state == State.ATTACK)
            {
                state = State.CHASE;
            }
        }

        private void AttackPlayer()
        {
            //agent.speed = attackSpeed;    // This causes issues

            //Vector3 trial = new Vector3(0, target.transform.position.y, 0);
            Vector3 trial = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            transform.LookAt(trial);    // Fucks up pos/ rotation
            agent.SetDestination(player.transform.position);

            // ChatGpt Trial: (Not sure if this even works)
            //Vector3 directionToPlayer = target.transform.position - transform.position;
            //directionToPlayer.y = 0;

            //if(directionToPlayer.y > 0.1f)
            //{
            //    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            //}
            // ChatGpt Trial

            if (!alreadyAttacked)
            {
                rb = Instantiate(rockObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

                rb.velocity = transform.forward * 30;
                StartCoroutine(projectileManager.deleteProjectile(rb.gameObject));

                alreadyAttacked = true;
                enemyAnimator.Play(attackAnimation);

                //StartCoroutine(ResetAttacked());  // For some reason doesn't work
                Invoke(nameof(ResetAttack), timeBetweenAttacks);    // CHANGE TO COROUTINE
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private IEnumerator ResetAttacked()
        {
            alreadyAttacked = false;
            yield return new WaitForSeconds(timeBetweenAttacks);
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
            transform.LookAt(player.transform.position);
            agent.SetDestination(player.transform.position);

            knifeObj.SetActive(false);

            enemyAnimator.StopPlayback();
            enemyAnimator.Play(runAnimation);
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