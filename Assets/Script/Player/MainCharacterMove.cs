using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMove : MonoBehaviour
{

    [SerializeField] float speed; 
    [SerializeField] float runSpeed; 

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigibody;
    Quaternion playerRotation;
    bool walking = false;
    bool walkingBackwards = false;

    int floorMask;
    float camRayLength = 100f; //Distancia de la camara

    float horiz = 0;
    float vert = 0;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor"); //Busca el string piso
        anim = GetComponent<Animator>();
        playerRigibody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        float currentSpeed = Run(vert);

        Move(horiz, vert, currentSpeed); //Movimiento

        Turning(); //Rotacion

        Animating(horiz, vert); //Animaciones
    }

    private void Move(float horiz, float vert, float currentSpeed)
    {
        movement = new Vector3(horiz, 0, vert);

        if (vert > 0)
        {
            movement = playerRotation * movement.normalized * currentSpeed * Time.deltaTime;
        }
        else if (vert < 0)
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

    private float Run(float vert)
    {
        // Solo corre si Shift está presionado y se mueve hacia adelante
        if (Input.GetKey(KeyCode.LeftShift) && vert > 0f)
            return runSpeed;
        else
            return speed;
    }

    private void Animating(float horiz, float vert)
    {
        walking = vert > 0f;
        walkingBackwards = vert < 0f;
        anim.SetBool("IsWalking", walking);
        anim.SetBool("IsWalkingBackwards", walkingBackwards);
    }
}
