using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] int damage; //Danio que hace el jugador
    [SerializeField] float timeBetweenBullets; //Tiempo que tarda en disparar el jugador
    [SerializeField] float range; //Rango de disparo del jugador, longitud del raycast
    [SerializeField] LayerMask shooteableMask; //Capas que se pueden disparar

    [Header("Cursors")]
    [SerializeField] private Texture2D cursorTextureAim;
    [SerializeField] private Texture2D cursorTextureHit;

    private Vector2 cursorHotspot;


    [Header("UI section")]
    [SerializeField] HoverOver _hoverOverToolbar;

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

        cursorHotspot = new Vector2(cursorTextureAim.width/2, cursorTextureHit.height/2);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; //Contador de tiempo

        ChangeCursor();
        
        if (Input.GetMouseButton(0) && timer >= timeBetweenBullets && (!_hoverOverToolbar.IsOverElement()))
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

    private void ChangeCursor()
    {
        ray.origin= transform.position;
        ray.direction= transform.forward;
        if(Physics.Raycast(ray, out hit, 0.7f, shooteableMask))
        {
            Cursor.SetCursor(cursorTextureHit, cursorHotspot, CursorMode.Auto);
        } else
        {
            Cursor.SetCursor(cursorTextureAim, cursorHotspot, CursorMode.Auto);
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
