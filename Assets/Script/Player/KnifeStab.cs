using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class KnifeStab : MonoBehaviour
{
    [SerializeField] int damage; //Danio que hace el cuchillo
    [SerializeField] float timeBetweenStabs; //Tiempo que tarda en apunialar el jugador
    [SerializeField] LayerMask stabMask; //Capas que se pueden apunialar
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth _enemyHealth = other.GetComponent<EnemyHealth>();
            Vector3 contact = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            _enemyHealth.TakeDamage(damage, contact);
        }
    }

    
}
