using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalHealth : MonoBehaviour
{
    //[SerializeField] float countHelath; //cantidad de salud que da el objeto
    [Header("Item MedKit")]
    [SerializeField] Item item;

    [Header("Components")]
    AudioSource audioSource;
    AudioClip audioClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioClip = audioSource.clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerHealth>())
        {
            //other.GetComponent<PlayerHealth>().ReceiveHealth(countHelath);
            bool canAdd = InventoryManager.instance.AddItem(item);
            if(canAdd)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position, 0.5f);
                Destroy(gameObject);
            }
        }
    }
}
