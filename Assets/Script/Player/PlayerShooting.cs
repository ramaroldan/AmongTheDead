using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] int damage; //Danio que hace el jugador
    [SerializeField] float timeBetweenBullets; //Tiempo que tarda en disparar el jugador
    [SerializeField] float range; //Rango de disparo del jugador, longitud del raycast
    [SerializeField] LayerMask shooteableMask; //Capas que se pueden disparar

    float timer; //Contador de tiempo
    Ray ray; //Rayo que dispara el jugador
    RaycastHit hit; //Objeto que se choca con el rayo
    LineRenderer lineRenderer; //Linea que se dibuja en el rayo
    AudioSource audioS; //Sonido que se reproduce al disparar
    Light gunLight; //Luz que se enciende al disparar
    float effectsDisplayTime = 0.2f; //Tiempo que tarda en desaparecer el rayo

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; //Contador de tiempo

        if (Input.GetMouseButtonDown(0) && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        //Desabilitar los efectos
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            lineRenderer.enabled = false;
            gunLight.enabled = false;
        }
        
    }

    void Shoot()
    {
        audioS.Play();

        timer = 0; //reiniciamos el contador
        lineRenderer.enabled = true; //habilitamos el componente lineRenderer
        gunLight.enabled = true; //habilitamos la luz
        lineRenderer.SetPosition(0, transform.position); //posicion inicial del rayo

        ray.origin = transform.position;
        ray.direction = transform.forward;

        if(Physics.Raycast(ray, out hit, range, shooteableMask))
        {
            //me guardo en una variable local el gameobject con el que estoy chocando
            GameObject _object = hit.collider.gameObject; //Objeto que se choca con el rayo

           
            lineRenderer.SetPosition(1, hit.point); //estableciendo la posicion final del rayo
            //compruebo si ese gameobject tiene el componente EnemyHealth
            if(_object.GetComponent<EnemyHealth>())
            {
                _object.GetComponent<EnemyHealth>().TakeDamage(damage, hit.point);//le aplico el danio
            }
        }
        else
        {
            lineRenderer.SetPosition(1, ray.origin + (ray.direction * range)); //posicion final del rayo
        }
    }
}
