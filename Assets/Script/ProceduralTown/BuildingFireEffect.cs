using UnityEngine;

public class BuildingFireEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] fireParticlePrefabs; // Prefabs de partículas alternativos
    [SerializeField, Range(0f, 1f)] private float spawnProbability = 1f; // Probabilidad de aparición
    [SerializeField] private Vector3 fireOffset = new Vector3(0, 0, 2f); // Offset en todas las direcciones
    [SerializeField] private float particleSize = 1f; // Tamaño de la partícula

    void Start()
    {
        // Comprueba la probabilidad de aparición
        if (fireParticlePrefabs.Length > 0 && Random.value <= spawnProbability)
        {
            // Selecciona un prefab aleatorio
            var prefab = fireParticlePrefabs[Random.Range(0, fireParticlePrefabs.Length)];

            if (prefab != null)
            {
                // Calcula la posición con offset configurable
                Vector3 firePosition = transform.position + transform.TransformDirection(fireOffset);
                var fire = Instantiate(prefab, firePosition, transform.rotation, transform);

                // Ajusta el tamaño de la partícula
                var particleSystem = fire.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var main = particleSystem.main;
                    main.startSize = particleSize;
                }
            }
            else
            {
                Debug.LogWarning("Uno de los prefabs de partículas de fuego no está asignado.");
            }
        }
        else if (fireParticlePrefabs.Length == 0)
        {
            Debug.LogWarning("No se han asignado prefabs de partículas de fuego.");
        }
    }
}