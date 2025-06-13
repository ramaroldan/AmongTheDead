using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMove : MonoBehaviour
{

    [SerializeField] float speed = 1f; //modificado en el editor para matchear con velocidad de animaciones
    [SerializeField] float runSpeed;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigibody;
    Quaternion playerRotation;
    //bool walking = false;
    //bool walkingBackwards = false;

    int floorMask;
    float camRayLength = 100f; //Distancia de la camara

    float horiz = 0f;
    float vert = 0f;

    private bool isStabbing = false;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor"); //Busca el string piso
        anim = GetComponent<Animator>();
        playerRigibody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        vert = Input.GetAxisRaw("Vertical");
        horiz = Input.GetAxisRaw("Horizontal");

        float currentSpeed = Run(vert);

        if (!isStabbing)
        {
            Move(horiz, vert, currentSpeed); //Movimiento

            Turning(); //Rotacion
        }

        Animating(vert, horiz, currentSpeed); //Animaciones
    }

    private float Run(float vert)
    {
        if(Input.GetKey(KeyCode.LeftShift) && vert > 0 && horiz ==0)
        {
            return runSpeed;
        } else
        {
            return speed;
        }
    }

    private void Move(float horiz, float vert, float currentSpeed)
    {
        movement = new Vector3(horiz, 0, vert);

        //movement = movement.normalized * speed * Time.deltaTime;

        if (vert > 0)
        {
            // vector movement ahora multiplicado por la rotación
            movement = playerRotation * movement.normalized * currentSpeed * Time.deltaTime;
        }
        else if (vert < 0)
        {
            // si el movimiento es <0 implica que nos movemos hacia atras y lo haremos mas lento (speed/2)
            movement = playerRotation * movement.normalized * currentSpeed / 2 * Time.deltaTime;
        } else // movimiento solamente en horizontal lo haremos mas lento (speed/2)
        {
            movement = playerRotation * movement.normalized * currentSpeed / 2 * Time.deltaTime;
        }
        playerRigibody.MovePosition(transform.position + movement);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Debug.DrawRay(camRay.origin, camRay.direction * 100f, Color.red);

            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigibody.MoveRotation(newRotation);
            // capturamos LookRotation para aplicar al movimiento del rigidbody
            playerRotation = newRotation;
        }
    }

    private void Animating(float v, float h, float speed)
    {
        //walkingBackwards = vert < 0f;
        //anim.SetBool("IsWalking", walking);
        //anim.SetBool("IsWalkingBackwards", walkingBackwards);
        anim.SetFloat("VelZ", v);
        anim.SetFloat("VelX", h);
        anim.SetFloat("RunSpeed", speed);
        //Debug.Log(speed);
    }

    public void Immobilize()
    {
        isStabbing= true;
    }
    public void Remobilize()
    {
        isStabbing= false;
    }
}
