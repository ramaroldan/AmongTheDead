using System.Collections;
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
        public float sideOffset = 1.5f;
        public float spawnProbability = 1f;

        public RoadHelper roadHelper;
        public StructureHelper structureHelper;

        void Start()
        {
            StartCoroutine(EsperarYInstanciar());
        }

        private IEnumerator EsperarYInstanciar()
        {
            // Espera hasta que haya al menos un edificio en el diccionario
            while (structureHelper == null || structureHelper.structuresDictionary.Count == 0)
            {
                yield return null;
            }
            InstanciarObjetos();
        }

        private void InstanciarObjetos()
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
    }
}