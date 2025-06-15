using System.Collections.Generic;
using UnityEngine;

public class ProceduralForestGenerator : MonoBehaviour
{
    [Header("Zona de generación")]
    public int width = 50;
    public int depth = 50;

    [Header("Densidad")]
    [Range(0.5f, 5f)]
    public float spacing = 1f; // Distancia entre árboles

    [Header("Prefabs")]
    public List<GameObject> treePrefabs;
    public List<GameObject> rockPrefabs;
    public GameObject bossPrefab;

    [Header("Porcentaje de rocas (0 a 1)")]
    [Range(0f, 1f)]
    public float rockChance = 0.1f;

    [Header("Altura mínima del terreno (Y)")]
    public float terrainHeight = 0f;

    void Start()
    {
        GenerateForest();
    }

    void GenerateForest()
    {
        Vector3 center = transform.position;

        for (float x = -width / 2f; x < width / 2f; x += spacing)
        {
            for (float z = -depth / 2f; z < depth / 2f; z += spacing)
            {
                Vector3 pos = new Vector3(center.x + x, terrainHeight, center.z + z);

                float chance = Random.value;
                if (chance < rockChance && rockPrefabs.Count > 0)
                {
                    InstantiateRandom(rockPrefabs, pos);
                }
                else if (treePrefabs.Count > 0)
                {
                    InstantiateRandom(treePrefabs, pos);
                }
            }
        }

        // Crear jefe final en el centro del bosque
        if (bossPrefab != null)
        {
            Vector3 bossPos = new Vector3(center.x, terrainHeight, center.z);
            Instantiate(bossPrefab, bossPos, Quaternion.identity);
        }
    }

    void InstantiateRandom(List<GameObject> prefabs, Vector3 position)
    {
        int index = Random.Range(0, prefabs.Count);
        GameObject obj = Instantiate(prefabs[index], position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        obj.transform.parent = this.transform; // Agrupa todo bajo el generador
    }
}
