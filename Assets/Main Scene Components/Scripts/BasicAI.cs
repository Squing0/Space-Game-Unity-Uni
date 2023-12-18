using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public enum State { PATROL, CHASE, SHIP, ATTACK}
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

    public GameObject bulletManagerObj;
    private BulletManager bm;
    private Rigidbody rb;
    
    private void Awake()
    {
        bm = bulletManagerObj.GetComponent<BulletManager>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        state = State.SHIP;
        StartCoroutine(FSM());
    }


    IEnumerator FSM()
    {
        while(true)
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
        if (PlayerWithinRange)
        {
            state = State.ATTACK;
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(target.transform.position);
        transform.LookAt(target.transform.position);

        if(!alreadyAttacked)
        {
            Vector3 alteredPosition = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z + .2f);

            rb = Instantiate(bulletManagerObj, alteredPosition, Quaternion.identity).GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 50;
            StartCoroutine(bm.deleteBullet(rb.gameObject));

            alreadyAttacked = true;

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
        agent.SetDestination(ship.transform.position);
    }
    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = State.CHASE;
            target = collision.gameObject;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
