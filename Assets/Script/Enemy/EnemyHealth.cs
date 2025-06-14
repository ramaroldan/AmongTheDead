using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth; //Salud maxima del enemigo
    [SerializeField] int currentHealth; //Salud actual del enemigo
    //[SerializeField] float sinkSpeed; //Velocidad de caída del enemigo
    [SerializeField] int scoreValue; //Puntuación del enemigo al morir
    public bool isDead; //enemigo muerto

    //[SerializeField] ParticleSystem hitParticles;

    [SerializeField] Slider slider; //barra de vida
    [SerializeField] Image fillImage; //imagen de la barra

    [SerializeField] AudioClip deathClip;

    AudioSource audioS;
    Animator anim;
    EnemyMovement _enemyMovement;
    bool isSinking; //si enemegio esta cayendo

    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth; //barra de vida maxima
        slider.value = maxHealth; //barra de vida actual

        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        _enemyMovement = GetComponent<EnemyMovement>();

        if (fillImage != null)
            fillImage.color = Color.green;
    }

    //funcion publica porque voy a llamarla desde el script de disparo del player
    public void TakeDamage(int amount, Vector3 point)
    {
        if (isDead) return; //si el enemigo esta muerto, salgo de la funcion

        currentHealth -= amount; //le resto la cantidad de daño al enemigo
        slider.value = currentHealth; //actualiza la barra de vida
        audioS.Play(); //reproduccion

        // Calcula el porcentaje de vida restante
        float healthPercent = (float)currentHealth / maxHealth;
        // Interpola entre verde (vida completa) y rojo (sin vida)
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);


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
        _enemyMovement.StopMoving(); // agregado para que el agent se detenga al morir
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false; //desactivamos la navegacion>
        Destroy(gameObject, 2f);
    }

}
