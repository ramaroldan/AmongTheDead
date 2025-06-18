using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPistol : MonoBehaviour
{
    [Header("Item Pistol")]
    public Item item;

    [Header("Components")]
    AudioSource audioSource;
    AudioClip audioClip;

    void Awake()
    {
        audioSource= GetComponent<AudioSource>();
        audioClip = audioSource.clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if(canAdd)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position, 0.5f);
                Destroy(gameObject);
            }
        }
    }
}
