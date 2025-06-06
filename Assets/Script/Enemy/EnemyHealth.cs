using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth; //Salud maxima del enemigo
    [SerializeField] int currentHealth; //Salud actual del enemigo
    //[SerializeField] float sinkSpeed; //Velocidad de caída del enemigo
    [SerializeField] int scoreValue; //Puntuación del enemigo al morir
    public bool isDead; //enemigo muerto

    //[SerializeField] ParticleSystem hitParticles;

    [SerializeField] AudioClip deathClip;

    AudioSource audioS;
    Animator anim;
    bool isSinking; //si enemegio esta cayendo

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        /*if (isSinking == true) //si el enemigo esta cayendo
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime); //se mueve hacia abajo
        }*/
    }

    //funcion publica porque voy a llamarla desde el script de disparo del player
    public void TakeDamage(int amount, Vector3 point)
    {
        if (isDead) return; //si el enemigo esta muerto, salgo de la funcion

        currentHealth -= amount; //le resto la cantidad de daño al enemigo
        audioS.Play(); //reproduccion

        //situo el sistema de particulas en el punto de impacto
        //hitParticles.transform.position = point; //posicion de las particulas
        //hitParticles.Play(); //emision de particulas
        
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        audioS.clip = deathClip;
        audioS.Play();

        isDead = true;
        anim.SetTrigger("Death");
        Destroy(gameObject, 3f);
        //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ScoreEnemy(scoreValue);
    }

   /* public void StartSinking() //funcion para que el enemigo caiga
    { 
        isSinking = true;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false; //desactivamos la navegacion>
        Destroy(gameObject, 2f);
    }*/
}
