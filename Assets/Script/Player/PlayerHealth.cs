using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth; //vida maxima
    [SerializeField] float currentHealth; //vida actual

    //otra forma de hacer vida con img (superior derecho)
    [SerializeField] Image healthImage;

    [Header("UI Health")]
    [SerializeField] Slider slider; //barra de vida
    [SerializeField] Image damegeImage; //imagen de daño
    [SerializeField] float flashSpeed = 5f; //velocidad de desaparocion de la imagen de daño
    [SerializeField] Color flashColour; //color de la imagen de daño

    [SerializeField] GameManager gameManager;

    [SerializeField] AudioClip deathClip;


    //Player Components
    AudioSource audioS; //audio
    Animator anim; //animaciones
    MainCharacterMove playerMovement; //movimiento
    PlayerShooting playerShooting; //disparo

    bool isDead; //muerto
    bool damaged; //daño

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //vida actual igual a la vida maxima
        slider.maxValue = maxHealth; //barra de vida maxima
        slider.value = maxHealth; //barra de vida actual

        //otra forma de hacer vida con img (superior derecho)
        healthImage.fillAmount = 1f;

        //obtenemos los componentes
        anim = GetComponent<Animator>(); //animaciones
        playerMovement = GetComponent<MainCharacterMove>(); //movimiento
        playerShooting = GetComponentInChildren<PlayerShooting>(); //disparo
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            damegeImage.color = flashColour; //color de la imagen de daño
        }
        else
        {
            damegeImage.color = Color.Lerp(damegeImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    //funcion que voy a llamar desde el script del enemigo
    public void TakeDamage(int amount) //recibe el daño
    {
        if (isDead) return; //si el jugador esta muerto, salgo de la funcion
        audioS.Play();
        damaged = true; //si me dañaron
        currentHealth -= amount; //resta el daño a la vida actual
        slider.value = currentHealth; //actualiza la barra de vida

        //otra forma de hacer vida con img (superior derecho)
        healthImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0) //si la vida actual es menor o igual a 0
        {
            Death(); //muere
        }
    }

    void Death()
    {
        audioS.clip = deathClip; //cambio audio 
        audioS.Play(); //reproduccion

        isDead = true;
        //anim.SetTrigger("Death");
        playerMovement.enabled = false; //desactivamos el movimiento
        playerShooting.enabled = false; //desactivamos el disparo
        gameManager.GameOver();
    }

    /*public void RestartLevel()
    {
        Debug.Log("Game Over");
    }*/
}
