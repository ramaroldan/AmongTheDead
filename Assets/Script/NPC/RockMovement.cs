using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockMovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    //EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //buscamos al player
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
        Animating();
    }

    void Animating()
    {
        if (agent.velocity.magnitude != 0)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude * 10);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    public void StopMoving()
    {
        agent.isStopped = true; // detiene el movimiento del agent 
    }

    public void ResumeMoving()
    {
        agent.isStopped = false; // permite el movimiento del agent
    }
}
