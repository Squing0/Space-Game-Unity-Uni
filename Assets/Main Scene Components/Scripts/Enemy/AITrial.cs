using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITrial : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Bullet
    public GameObject bulletManagerObj;
    private ProjectileManager pm;
    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        pm = bulletManagerObj.GetComponent<ProjectileManager>();    // REMEMBER TO DELETE THIS SCRIPTTTT
    }
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInSightRange && !playerInAttackRange) { Patrolling(); }
        if (playerInSightRange && !playerInAttackRange) { ChasePlayer(); }
        if (playerInSightRange && playerInAttackRange) { AttackPlayer(); }

    }

    private void Patrolling()
    { 
        if(!walkPointSet) { searchWalkPoint(); }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void searchWalkPoint()
    {
        //Calcualte rnadom point in range
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, 
            transform.position.y, 
            transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer() 
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer() 
    {
        // Makes enemy stop moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 20f, ForceMode.Impulse);
            rb.velocity = transform.forward * 50;

            StartCoroutine(pm.deleteProjectile(rb.gameObject));

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetBullet(GameObject bulletClone)
    {
        Destroy(bulletClone);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void ResetAttack() 
    {
        alreadyAttacked = false;
    }
}
