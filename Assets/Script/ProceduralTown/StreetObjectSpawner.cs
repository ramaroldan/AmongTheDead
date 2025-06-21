using System.Collections.Generic;
using UnityEngine;

namespace SVS
{
    public class StreetObjectSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject trafficLightPrefab;
        public GameObject lightPolePrefab;
        public GameObject trashCanPrefab;
        public GameObject barrierPrefab;
        public GameObject carPrefab;

        [Header("Configuración")]
        public float sideOffset = 1.5f; // Distancia del borde de la calle para palos de luz
        public float spawnProbability = 1f; // Para pruebas, instanciar siempre

        public RoadHelper roadHelper;
        public StructureHelper structureHelper; // Referencia al StructureHelper

        void Awake()
        {
            if (roadHelper == null)
            {
                Debug.LogWarning("No se asignó RoadHelper en StreetObjectSpawner.");
                return;
            }
            roadHelper.finishedCoroutine += OnRoadGenerationFinished;
        }

        private void OnDestroy()
        {
            if (roadHelper != null)
            {
                roadHelper.finishedCoroutine -= OnRoadGenerationFinished;
            }
        }

        private void OnRoadGenerationFinished()
        {
            Debug.Log("OnRoadGenerationFinished llamado");
            List<Vector3Int> roadPositions = roadHelper.GetRoadPositions();
            SpawnObjects(roadPositions);
        }

        public void SpawnObjects(List<Vector3Int> roadPositions)
        {
            if (structureHelper != null)
            {
                Debug.Log("Cantidad de edificios: " + structureHelper.structuresDictionary.Count);
                int count = 0;
                foreach (var kvp in structureHelper.structuresDictionary)
                {
                    GameObject buildingObj = kvp.Value;
                    Vector3 buildingWorldPos = buildingObj.transform.position;
                    Quaternion buildingRot = buildingObj.transform.rotation;

                    if (lightPolePrefab != null && Random.value < spawnProbability)
                    {
                        Debug.Log("Instanciando palo de luz");
                        Vector3 rightOffset = buildingObj.transform.right * sideOffset;
                        Vector3 posRight = buildingWorldPos + rightOffset;
                        posRight.y = buildingWorldPos.y;

                        var rightPole = Instantiate(lightPolePrefab, posRight, buildingRot, transform);
                        Debug.Log($"Palo de luz instanciado en {posRight} con rotación {buildingRot.eulerAngles}");
                        count++;
                    }
                }
                Debug.Log($"Total de palos de luz instanciados: {count}");
            }
            else
            {
                Debug.LogWarning("No se asignó StructureHelper en StreetObjectSpawner.");
            }
        }
    }
}