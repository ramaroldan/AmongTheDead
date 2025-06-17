using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPistol : MonoBehaviour
{
    [Header("Item Pistol")]
    public Item item;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if(canAdd )
            {
                Destroy(gameObject);
            }
        }
    }
}
