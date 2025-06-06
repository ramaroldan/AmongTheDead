using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Tooltip("Transform del jugador a perseguir")]
    public Transform playerTransform;

    [Tooltip("Distancia m�nima para empezar a perseguir")]
    public float chaseDistance = 10f;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= chaseDistance)
        {
            // Persigue al jugador
            _agent.SetDestination(playerTransform.position);
        }
        else
        {
            // Se queda en su posici�n inicial o patrulla (opcional)
            _agent.SetDestination(transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el radio de persecuci�n en el Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
