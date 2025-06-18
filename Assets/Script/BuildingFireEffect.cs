using UnityEngine;

public class BuildingFireEffect : MonoBehaviour
{
    [SerializeField] private GameObject fireParticlePrefab;
    [SerializeField] private float fireOffset = 2f; // Distancia delante del edificio

    void Start()
    {
        if (fireParticlePrefab != null)
        {
            // Calcula la posición delante del edificio
            Vector3 firePosition = transform.position + transform.forward * fireOffset;
            var fire = Instantiate(fireParticlePrefab, firePosition, transform.rotation, transform);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el prefab de partículas de fuego.");
        }
    }
}