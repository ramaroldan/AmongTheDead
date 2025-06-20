using System.Collections.Generic;
using UnityEngine;

namespace SVS
{
    public class StreetObjectSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject trafficLightPrefab; // Semáforo
        public GameObject lightPolePrefab; // Palos de luz
        public GameObject trashCanPrefab; // Tachos de basura
        public GameObject barrierPrefab; // Barreras
        public GameObject carPrefab; // Autos

        [Header("Configuración")]
        public float sideOffset = 2.5f; // Distancia desde el centro de la calle al costado
        public int lightPoleSpacing = 5; // Cada cuántos metros poner un palo de luz
        public int objectSpacing = 7;    // Espaciado para otros objetos

        public RoadHelper roadHelper;

        private void Start()
        {
            if (roadHelper == null)
            {
                roadHelper = FindObjectOfType<RoadHelper>();
            }
            if (roadHelper != null)
            {
                roadHelper.finishedCoroutine += SpawnObjects;
            }
        }

        public void SpawnObjects()
        {
            if (roadHelper == null) return;

            List<Vector3Int> roadPositions = roadHelper.GetRoadPositions();
            var roadSet = new HashSet<Vector3Int>(roadPositions);
            int index = 0;

            foreach (var pos in roadPositions)
            {
                // Detectar la dirección principal de la calle en esta posición
                List<Direction> directions = PlacementHelper.FindNeighbour(pos, roadSet);

                // Por defecto, la calle va en X (horizontal)
                Vector3 sideDir = Vector3.forward; // Z
                if (directions.Count == 2)
                {
                    if ((directions.Contains(Direction.Up) && directions.Contains(Direction.Down)))
                        sideDir = Vector3.right; // Calle vertical, costado en X
                    else if ((directions.Contains(Direction.Left) && directions.Contains(Direction.Right)))
                        sideDir = Vector3.forward; // Calle horizontal, costado en Z
                    else if (directions.Contains(Direction.Up) && directions.Contains(Direction.Right))
                        sideDir = (Vector3.left + Vector3.back).normalized;
                    else if (directions.Contains(Direction.Right) && directions.Contains(Direction.Down))
                        sideDir = (Vector3.left + Vector3.forward).normalized;
                    else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Down))
                        sideDir = (Vector3.right + Vector3.forward).normalized;
                    else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Up))
                        sideDir = (Vector3.right + Vector3.back).normalized;
                }
                else if (directions.Count == 1)
                {
                    // Calle termina, tomar dirección opuesta como costado
                    if (directions.Contains(Direction.Up) || directions.Contains(Direction.Down))
                        sideDir = Vector3.right;
                    else
                        sideDir = Vector3.forward;
                }

                // Instanciar objetos sobre la calle
                if (index % objectSpacing == 0)
                {
                    Instantiate(barrierPrefab, pos, Quaternion.identity, transform);
                    Instantiate(carPrefab, pos + sideDir * 0.5f, Quaternion.identity, transform);
                }

                // Instanciar palos de luz a los costados
                if (index % lightPoleSpacing == 0)
                {
                    Vector3 leftSide = pos + Quaternion.Euler(0, 90, 0) * sideDir * sideOffset;
                    Vector3 rightSide = pos + Quaternion.Euler(0, -90, 0) * sideDir * sideOffset;
                    Instantiate(lightPolePrefab, leftSide, Quaternion.identity, transform);
                    Instantiate(lightPolePrefab, rightSide, Quaternion.identity, transform);
                }

                // Instanciar tachos de basura y semáforos aleatoriamente a los costados
                if (Random.value < 0.1f)
                {
                    Vector3 side = pos + ((Random.value > 0.5f ? 1 : -1) * (Quaternion.Euler(0, 90, 0) * sideDir) * sideOffset);
                    Instantiate(trashCanPrefab, side, Quaternion.identity, transform);
                }
                if (Random.value < 0.05f)
                {
                    Vector3 side = pos + ((Random.value > 0.5f ? 1 : -1) * (Quaternion.Euler(0, 90, 0) * sideDir) * sideOffset);
                    Instantiate(trafficLightPrefab, side, Quaternion.identity, transform);
                }

                index++;
            }
        }
    }
}