using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralForestGenerator : MonoBehaviour
{
    [Header("Zona de generación")]
    [SerializeField] int width = 50;
    [SerializeField] int depth = 50;

    [Header("Densidad")]
    [Range(0.5f, 5f)]
    [SerializeField] float spacing = 1f; // Distancia entre árboles

    [Header("Prefabs")]
    [SerializeField] List<GameObject> treePrefabs;
    [SerializeField] List<GameObject> rockPrefabs;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject groundPrefab;

    [Header("Porcentaje de rocas (0 a 1)")]
    [Range(0f, 1f)]
    [SerializeField] float rockChance = 0.1f;

    [Header("Altura mínima del terreno (Y)")]
    [SerializeField] float terrainHeight = 0f;

    [Header("Tiempo de aparición/reaparición del boss (segundos)")]
    public float bossRespawnTime = 10f;

    private GameObject currentBoss;

    void Start()
    {
        GenerateForest();
        if (bossPrefab != null)
        {
            StartCoroutine(BossSpawnRoutine());
        }
    }

    void GenerateForest()
    {
        Vector3 center = transform.position;

        // Instanciar el ground si está asignado
        if (groundPrefab != null)
        {
            Vector3 groundPos = new Vector3(center.x, terrainHeight - 0.01f, center.z);
            GameObject ground = Instantiate(groundPrefab, groundPos, Quaternion.identity, this.transform);

            // Ajustar el tamaño del ground para cubrir el área (asume un plane de 10x10 unidades)
            float scaleX = width / 10f;
            float scaleZ = depth / 10f;
            ground.transform.localScale = new Vector3(scaleX, 1, scaleZ);
        }

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
    }

    void InstantiateRandom(List<GameObject> prefabs, Vector3 position)
    {
        int index = Random.Range(0, prefabs.Count);
        GameObject obj = Instantiate(prefabs[index], position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        obj.transform.parent = this.transform; // Agrupa todo bajo el generador
    }

    IEnumerator BossSpawnRoutine()
    {
        Vector3 center = transform.position;
        Vector3 bossPos = new Vector3(center.x, terrainHeight, center.z);

        // Espera antes de crear el boss por primera vez
        yield return new WaitForSeconds(bossRespawnTime);

        currentBoss = Instantiate(bossPrefab, bossPos, Quaternion.identity);

        // Luego sigue comprobando para respawn
        while (true)
        {
            if (currentBoss == null)
            {
                yield return new WaitForSeconds(bossRespawnTime);
                currentBoss = Instantiate(bossPrefab, bossPos, Quaternion.identity);
            }
            yield return null;
        }
    }
}