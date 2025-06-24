using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion prefab")]
    [SerializeField] private GameObject explosionEffectPrefab; //reference to explosion effect prefab
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0, 0, 0);

    [Header("Explosion settings")]
    [SerializeField] private float explosionDelay = 3f; // delay befor explosion
    [SerializeField] private float explosionForce = 10f; // force applied by explosion
    [SerializeField] private float explosionRadius = 2f; // radius of explosion
    [SerializeField] private int damage = 40;

    [Header("Audio effects")]

    private float countDown;
    private bool hasExploded = false;

    private void Start()
    {
        countDown = explosionDelay;
    }

    private void Update()
    {
        if (!hasExploded)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);

        Destroy(explosionEffect, 4f);

        // play sound effect?

        NearbyForceApply();

        Destroy(gameObject);
    }

    void NearbyForceApply()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && nearbyObject.CompareTag("Enemy"))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                EnemyHealth _enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                _enemyHealth.TakeDamage(damage, rb.position);
            }
            /*
            else
            {
                GameObject otherGameObject = nearbyObject.gameObject;
                if (otherGameObject.CompareTag("Enemy"))
                {
                    otherGameObject.AddComponent<Rigidbody>();
                    Rigidbody otherRb = otherGameObject.GetComponent<Rigidbody>();
                    otherRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    EnemyHealth _enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                    _enemyHealth.TakeDamage(damage, rb.position);
                }

            } */
        }
    }
}
