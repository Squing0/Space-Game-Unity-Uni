using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public enum State { PATROL, CHASE, SHIP}
    public State state;

    //public GameObject[] waypoints;
    //private int waypointIndex = 0;

    public GameObject ship;

    public float patrolSpeed = 0.5f;
    public float chaseSpeed = 1.0f;
    public GameObject target;

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
            }

            yield return null;
        }
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

}
