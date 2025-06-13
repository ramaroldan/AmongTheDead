using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //buscamos al player
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if(player != null && enemyHealth.isDead == false)
        {
            agent.SetDestination(player.transform.position);
        }
        Animating();
    }

    void Animating()
    {
        if (agent.velocity.magnitude != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public void StopMoving()
    {
        agent.isStopped = true; // detiene el movimiento del agent 
    }
}
