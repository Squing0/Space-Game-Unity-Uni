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

        Vector3 playerRange;

        [Header("Animations")]
        public Animator enemyAnimator;
        public String runAnimation;
        public String combatRunAnimation;
        public String attackAnimation;

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

            Collider knifeCollider = knifeObj.GetComponent<Collider>();
            knifeCollider.transform.position = knifeObj.transform.position;
            knifeCollider.transform.rotation = knifeObj.transform.rotation;
        }

        private void AttackPlayer()
        {
            agent.SetDestination(target.transform.position);
            transform.LookAt(target.transform.position);  // Find better way for this
                                                          //enemyAnimator.Play(combatRunAnimation);

            if (!alreadyAttacked)
            {
                rb = Instantiate(rockObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                //rb.gameObject.tag = "EnemyBullet";  // Changed tag as bullets would hit enemy colliders and weren't supposed to.    

                rb.velocity = transform.forward * 30;
                StartCoroutine(pm.deleteProjectile(rb.gameObject));

                alreadyAttacked = true;
                enemyAnimator.Play(attackAnimation);

                //StartCoroutine(ResetAttacked());
                Invoke(nameof(ResetAttack), 2f);
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
            agent.SetDestination(target.transform.position);
            knifeObj.SetActive(false);

            enemyAnimator.StopPlayback();
            enemyAnimator.Play(runAnimation);
        }


        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.tag == "Player") Can use if needed
            //{
            //    state = State.CHASE;
            //    target = collision.gameObject;
            //}
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}