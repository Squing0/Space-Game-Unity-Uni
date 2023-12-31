using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class BasicAi : MonoBehaviour
    {
        public NavMeshAgent agent;

        public enum State { PATROL, CHASE, SHIP, ATTACK }    // Make property? Might be different with enums?
        public State state;

        //public GameObject[] waypoints;
        //private int waypointIndex = 0;

        public GameObject ship;

        public float patrolSpeed = 0.5f;
        public float chaseSpeed = 1.0f;
        public GameObject target;

        public LayerMask isPlayer;
        public float attackRange;
        public bool PlayerWithinRange;
        public bool alreadyAttacked;

        public GameObject rockObj;
        private ProjectileManager pm;
        private Rigidbody rb;

        public GameObject knifeObj;
        public float timeBetweenAttacks;

        Vector3 playerRange;

        [Header("Animations")]
        public Animator enemyAnimator;
        public String runAnimation; // Change to lower case string
        public String combatRunAnimation;
        public String attackAnimation;

        Vector3 playerPos;
        public LayerMask whatisGround;

        private void Awake()
        {
            pm = rockObj.GetComponent<ProjectileManager>();
        }
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true;
            agent.updateRotation = true;

            //state = State.CHASE;  // fucks with instantiating states
            StartCoroutine(FSM());

            playerRange = new Vector3(ship.transform.position.x - 1,    // CHANGE NAME
                ship.transform.position.y,
                ship.transform.position.z - 1);
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

            //Collider knifeCollider = knifeObj.GetComponent<Collider>(); // THIS DOESN'T WORK
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
            //transform.LookAt(target.transform.position);
            agent.SetDestination(target.transform.position);

            // ChatGpt Trial:
            Vector3 directionToPlayer = target.transform.position - transform.position;
            directionToPlayer.y = 0;

            if(directionToPlayer.y > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            }
                   
            if (!alreadyAttacked)
            {
                rb = Instantiate(rockObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                //rb.gameObject.tag = "EnemyBullet";  // Changed tag as bullets would hit enemy colliders and weren't supposed to.    

                rb.velocity = transform.forward * 30;
                StartCoroutine(pm.deleteProjectile(rb.gameObject));

                alreadyAttacked = true;
                enemyAnimator.Play(attackAnimation);

                //StartCoroutine(ResetAttacked());
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
            yield return new WaitForSeconds(2f);
        }

        //private void patrol()
        //{
        //    agent.speed = patrolSpeed;
        //    if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) > 1) {
        //        agent.SetDestination(waypoints[waypointIndex].transform.position);
        //    }
        //    else
        //    {
        //        waypointIndex++;
        //    }

        //    if (waypointIndex >= waypoints.Length){
        //        waypointIndex = 0;
        //    }
        //}
        private void Ship()
        {
            knifeObj.SetActive(true);   // Should only have to be in one place
            agent.SetDestination(ship.transform.position);

            //var rotation = Quaternion.LookRotation(ship.transform.position - transform.position); // Taken from unity thread
            //rotation.y = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            //transform.LookAt(ship.transform.position);

            Rigidbody enemyRB = agent.GetComponent<Rigidbody>();
            enemyRB.constraints = RigidbodyConstraints.FreezeRotation;
            enemyRB.constraints = RigidbodyConstraints.FreezePosition;
            //enemyAnimator.Play(runAnimation);

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
            transform.LookAt(target.transform.position);
            agent.SetDestination(target.transform.position);
            

            //transform.LookAt(playerPos);
            //agent.SetDestination(playerPos);

            knifeObj.SetActive(false);

            enemyAnimator.StopPlayback();
            enemyAnimator.Play(runAnimation);
        }
     
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

            }
        }
    }
}