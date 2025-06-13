using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalHealth : MonoBehaviour
{
    [SerializeField] float countHelath; //cantidad de salud que da el objeto

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerHealth>())
        {
            other.GetComponent<PlayerHealth>().ReceiveHealth(countHelath);
            Destroy(gameObject);
        }
    }
}
