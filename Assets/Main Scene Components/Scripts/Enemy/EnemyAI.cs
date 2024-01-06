using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public NavMeshAgent agent;

        public enum State { PATROL, CHASE, SHIP, ATTACK }    // Make property? Might be different with enums?
        public State state;

        public GameObject ship;

        public float patrolSpeed = 0.5f;
        public float chaseSpeed = 1.0f;
        public GameObject player;

        public LayerMask isPlayer;
        public float attackRange;
        public bool PlayerWithinRange;
        public bool alreadyAttacked;

        public GameObject rockObj;
        private ProjectileManager projectileManager;
        private Rigidbody rb;

        public GameObject knifeObj;
        private Collider knifeCollider;
        public float timeBetweenAttacks;

        //Vector3 playerRange;

        [Header("Animations")]
        public Animator enemyAnimator;
        public string runAnimation; 
        public string combatRunAnimation;
        public string attackAnimation;

        Vector3 playerPos;
        public LayerMask whatisGround;

        private void Awake()
        {
            projectileManager = rockObj.GetComponent<ProjectileManager>();
        }
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.updatePosition = true;
            agent.updateRotation = true;

            knifeCollider = knifeObj.GetComponent<Collider>();

            StartCoroutine(FSM());

            //playerRange = new Vector3(ship.transform.position.x - 1,    // CHANGE NAME (use to fix ship AI?)
            //    ship.transform.position.y,
            //    ship.transform.position.z - 1);
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

           // THIS DOESN'T WORK
            //knifeCollider.transform.position = knifeObj.transform.position;
            //knifeCollider.transform.rotation = knifeObj.transform.rotation;

            //RaycastHit hit;
            //playerPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

            //if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatisGround))
            //{
            //   playerPos.y = hit.point.y;
            //}
            //Debug.DrawRay(transform.position, Vector3.down, Color.blue);
        }

        private void AttackPlayer()
        {
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
                //rb.gameObject.tag = "EnemyBullet";  // Changed tag as bullets would hit enemy colliders and weren't supposed to.    

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

            //var rotation = Quaternion.LookRotation(ship.transform.position - transform.position); // Taken from unity thread
            //rotation.y = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            //transform.LookAt(ship.transform.position);

            //Rigidbody enemyRB = agent.GetComponent<Rigidbody>();
            //enemyRB.constraints = RigidbodyConstraints.FreezeRotation;
            //enemyRB.constraints = RigidbodyConstraints.FreezePosition;
            //enemyAnimator.Play(runAnimation);

            if (!alreadyAttacked)   // Just copied from attack method
            {
                //knifeCollider.transform.position = knifeObj.transform.position;
                //knifeCollider.transform.rotation = knifeObj.transform.rotation;

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