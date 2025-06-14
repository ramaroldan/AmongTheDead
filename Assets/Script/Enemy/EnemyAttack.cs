using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks; //tiempo entre ataques
    [SerializeField] int attackDamage; //daño del ataque
    [SerializeField] private Collider _handCollider;

    Animator anim;
    GameObject player; //objeto player
    PlayerHealth playerHealth; //vida del player
    EnemyHealth enemyHealth; //vida del enemigo
    bool playerInRange; //bool para saber si el player esta en rango
    float timer; //tiempo

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime; //Contador de tiempo
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.isDead == false) //si el contador es mayor que el tiempo entre ataques y el player esta en rango
        { 
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) // si el objeto que entra es el player
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Attack()
    {
        Animating();
        timer = 0;
        //playerHealth.TakeDamage(attackDamage); //le hacemos daño al player

    }

    void Animating()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void EnableHandCollider()
    {
        _handCollider.enabled = true;
    }

    public void DisableHandCollider()
    {
        _handCollider.enabled = false;
    }

    public int getAttackDamage()
    {
        return attackDamage;
    }
}
