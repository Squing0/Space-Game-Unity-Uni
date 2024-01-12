using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    // Handles all variables and methods to manipulate enemy AI.
    public class EnemyAI : MonoBehaviour
    {
        [Header("Agent")]
        public NavMeshAgent agent;
        public enum State { PATROL, CHASE, SHIP, ATTACK }    // Enum used as values are unchangable.
        public State state;

        [Header("State speeds")]
        public float patrolSpeed = 0.5f;
        public float chaseSpeed = 1.0f;
        public float attackSpeed = 1.1f;

        [Header("Attack")]
        public LayerMask isPlayer;
        public float attackRange;
        public bool PlayerWithinRange;
        public bool alreadyAttacked;
        public float timeBetweenAttacks;

        [Header("Patrol")]
        public float patrolRange;
        public float timeBetweenPatrols;

        [Header("Other game objects")]
        public GameObject ship;
        public GameObject playerObj;
        public GameObject rockObj;
        public GameObject knifeObj;
        

        [Header("Animations")]
        public Animator enemyAnimator;
        public string runAnimation;
        public string walkAnimation;
        public string combatRunAnimation;
        public string attackAnimation;

        // Private patrol variables.
        private Vector3 newPatrolPos;
        private bool patrolPointFound;

        // Scripts to be manipualted.
        private ProjectileManager projectileManager;
        private GunShoot gunShoot;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            // Enemy position and rotation update as move.
            agent.updatePosition = true;
            agent.updateRotation = true;

            // Patrol point initially found so new patrol point can be found
            patrolPointFound = true;
        }
        private void Start()
        {
            projectileManager = rockObj.GetComponent<ProjectileManager>();
            gunShoot = FindAnyObjectByType<GunShoot>();

            // Coroutine called in start so continually runs.
            StartCoroutine(FSM());
        }

        // Controls when states are changed and calls assigned methods to states.
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
            ChangeStateIfRange();
            StopEnemy();
        }
        // changes state based on if player is in range.
        private void ChangeStateIfRange()
        {
            // Checks position of player relative to specified amount of distance to enemy.
            PlayerWithinRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if (PlayerWithinRange && state == State.CHASE)  // If player in enemy range and chasing, attack.
            {
                state = State.ATTACK;
            }
            else if (!PlayerWithinRange && state == State.ATTACK)   // If player not in enemy range, return to chase state.
            {
                state = State.CHASE;
            }

            // If player within range and player has no bullets at all, enemies will start attacking player.
            if (PlayerWithinRange && state == State.SHIP && gunShoot.CurrentBullets == 0 && gunShoot.bulletTotal == 0)
            {
                state = State.ATTACK;
            }
        }
        // Stops enemy if too close to player.
        private void StopEnemy()
        {
            // Checks if player is too close in attack range.
            bool playerTooClose = Physics.CheckSphere(transform.position, 9, isPlayer);

            // If player is too close then enemy stops moving.
            if (playerTooClose && state == State.ATTACK)
            {
                agent.speed = 0;
            }
            else    // if player not too close, then speed is set to normal.
            {
                agent.speed = chaseSpeed;
            }
        }
        // Enemy attacks player with rock object.
        private void AttackPlayer()
        {      
            // Enemy goes to directly to player.
            transform.LookAt(playerObj.transform.position); // Helps with aiming at player.
            agent.SetDestination(playerObj.transform.position);

            if (!alreadyAttacked)
            {
                // Instantiates rock object and fires toward player at specified speed.
                Rigidbody rb = Instantiate(rockObj, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.velocity = transform.forward * 30;

                // Resets attack.
                alreadyAttacked = true;
                StartCoroutine(ResetAttacked());

                // Starts coroutine to destrpoy projectile if not triggered or collided with objects.
                StartCoroutine(projectileManager.deleteProjectile(rb.gameObject));

                enemyAnimator.Play(attackAnimation);

            }
        }
        // Resets attack at specified time interval.
        private IEnumerator ResetAttacked()
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            alreadyAttacked = false;       
        }
     
        // Enemy attacks ship object.
        private void Ship()
        {
            // Knife only needed for ship object, so set here.
            knifeObj.SetActive(true);   
            agent.SetDestination(ship.transform.position);

            enemyAnimator.Play(attackAnimation);    // Knife animation played to simulate hitting ship.
        }
        // Enemy chases player.
        private void Chase()
        {           
            agent.speed = chaseSpeed;
            knifeObj.SetActive(false);  // Set false in case state changes from ship where knife active.

            // Enemy chases player directly.
            transform.LookAt(playerObj.transform.position);
            agent.SetDestination(playerObj.transform.position);

            enemyAnimator.StopPlayback();
            enemyAnimator.Play(runAnimation);
        }

        // Enemy patrols around area.
        private void Patrol()
        {
            agent.speed = patrolSpeed;
            enemyAnimator.Play(walkAnimation);

            if(patrolPointFound)    // Enemy continually looks for new patrol point, if one is found.
            {
                // New patrol point is found and set
                newPatrolPos = FindNewPatrolPoint();    
                agent.SetDestination(newPatrolPos);

                // Patrol point is reset.
                patrolPointFound = false;
                StartCoroutine(ResetpatrolPos());
            }
        }

        // Patrol point is reset after specified time.
        private IEnumerator ResetpatrolPos()
        {
            yield return new WaitForSeconds(timeBetweenPatrols); 
            patrolPointFound = true;
        }

        // Enemy looks for new patrol point. Gotten from YouTube tutorial.
        private Vector3 FindNewPatrolPoint()    
        {
            // X and Z values are randomly generated in specific range.
            float randomx = Random.Range(-patrolRange, patrolRange);
            float randomz = Random.Range(-patrolRange, patrolRange);

            // Patrol point with new X and Z values are returned.
            return new Vector3(gameObject.transform.position.x + randomx, 
                0, 
                gameObject.transform.position.z + randomz);           
        }

        // Increases enemy speed by specified amount for a specified time.
        public void SpeedUpActivate(float speedIncrease, float speedTime)   
        {
            agent.speed += speedIncrease;
            StartCoroutine(SpeedTimer(speedIncrease, speedTime));
        }
        // Returns enemy speed to normal after specified time.
        private IEnumerator SpeedTimer(float speedDecrease, float speedCooldown)
        {
            yield return new WaitForSeconds(speedCooldown);
            agent.speed -= speedDecrease;
        }
    }
}