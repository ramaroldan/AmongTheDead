using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClaw : MonoBehaviour
{
    [SerializeField] private int attackDamage; //danio del ataque
    GameObject player;
    [SerializeField] BossAttack bossAttack;
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

            _playerHealth.TakeDamage(bossAttack.GetAttackDamage());
        }
    }
}
