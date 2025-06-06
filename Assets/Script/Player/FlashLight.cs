using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    Light lightPlayerComponent;

    void Start()
    {
        lightPlayerComponent = GetComponent<Light>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            lightPlayerComponent.enabled = !lightPlayerComponent.enabled;
        }
    }
}
