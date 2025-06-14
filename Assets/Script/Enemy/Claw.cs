using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [SerializeField] int attackDamage; //danio del ataque
    GameObject player;
    [SerializeField] EnemyAttack enemyAttack;
    PlayerHealth _playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = player.GetComponent<PlayerHealth>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            _playerHealth.TakeDamage(enemyAttack.getAttackDamage());
        }
    }

}
