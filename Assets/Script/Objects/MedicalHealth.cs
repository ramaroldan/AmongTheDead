using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalHealth : MonoBehaviour
{
    //[SerializeField] float countHelath; //cantidad de salud que da el objeto
    [SerializeField] Item item;

    AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    /*private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            audioS.Play();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerHealth>())
        {
            //other.GetComponent<PlayerHealth>().ReceiveHealth(countHelath);
            bool canAdd = InventoryManager.instance.AddItem(item);
            if(canAdd)
            {
                Destroy(gameObject);
            }
        }
    }
}
