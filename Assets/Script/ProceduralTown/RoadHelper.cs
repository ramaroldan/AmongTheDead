using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace SVS
{
    [Serializable]
    public class StreetObjectConfig
    {
        public string name;
        public List<GameObject> prefabs;
        [Range(0, 1)] public float spawnProbability = 0.3f;
        public Vector3 offset = Vector3.zero;
        public Vector2 randomRange = Vector2.zero;
        public Vector3 rotationOffset = Vector3.zero;
    }

    public class RoadHelper : MonoBehaviour
    {
        public Action finishedCoroutine;
        [SerializeField] GameObject roadStrainght;
        [SerializeField] GameObject roadCorner;
        [SerializeField] GameObject road3Way;
        [SerializeField] GameObject road4Way;
        [SerializeField] GameObject roadEnd;

        // Prefabs de autos y parámetros
        [Header("Autos")]
        [SerializeField] List<GameObject> carPrefabs;
        [SerializeField, Range(0, 1)] float carSpawnProbability = 0.3f;
        [SerializeField] Vector3 carOffset = new Vector3(0, 0.1f, 0);
        [SerializeField] Vector2 carRandomRange = new Vector2(-0.3f, 0.3f);
        [SerializeField] Vector3 carRotationOffset = Vector3.zero; // Offset de rotación en grados

        [Header("Objetos de calle")]
        [SerializeField] List<StreetObjectConfig> streetObjects;

        public float animationTime = 0.01f;

        Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
        HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

        public List<Vector3Int> GetRoadPositions()
        {
            return roadDictionary.Keys.ToList();
        }

        public IEnumerator PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int lenght)
        {
            var rotation = Quaternion.identity;
            if (direction.x == 0)
            {
                rotation = Quaternion.Euler(0, 90, 0);
            }

            for (int i = 0; i < lenght; i++)
            {
                var position = Vector3Int.RoundToInt(startPosition + direction * i);
                if (roadDictionary.ContainsKey(position))
                {
                    continue;
                }
                var road = Instantiate(roadStrainght, position, rotation, transform);
                roadDictionary.Add(position, road);

                if (i == 0 || i == lenght - 1)
                {
                    fixRoadCandidates.Add(position);
                }

                // Instanciar autos y objetos de calle solo si NO es la última calle
                if (i < lenght - 1)
                {
                    TrySpawnCar(position, rotation);

                    if (streetObjects != null)
                    {
                        foreach (var config in streetObjects)
                        {
                            TrySpawnStreetObject(config, position, rotation);
                        }
                    }
                }

                yield return new WaitForSeconds(animationTime);
            }
            finishedCoroutine?.Invoke();
        }

        // Método para instanciar autos
        void TrySpawnCar(Vector3Int roadPosition, Quaternion roadRotation)
        {
            if (carPrefabs == null || carPrefabs.Count == 0) return;
            if (UnityEngine.Random.value > carSpawnProbability) return;

            // Selecciona un prefab aleatorio
            var carPrefab = carPrefabs[UnityEngine.Random.Range(0, carPrefabs.Count)];

            // Calcula offset y posición aleatoria
            float offsetX = UnityEngine.Random.Range(carRandomRange.x, carRandomRange.y);
            float offsetZ = UnityEngine.Random.Range(carRandomRange.x, carRandomRange.y);
            Vector3 spawnPos = (Vector3)roadPosition + carOffset + new Vector3(offsetX, 0, offsetZ);

            // Aplica el offset de rotación
            Quaternion finalRotation = roadRotation * Quaternion.Euler(carRotationOffset);

            Instantiate(carPrefab, spawnPos, finalRotation, transform);
        }

        // Método para instanciar objetos de calle genéricos
        void TrySpawnStreetObject(StreetObjectConfig config, Vector3Int roadPosition, Quaternion roadRotation)
        {
            if (config.prefabs == null || config.prefabs.Count == 0) return;
            if (UnityEngine.Random.value > config.spawnProbability) return;

            var prefab = config.prefabs[UnityEngine.Random.Range(0, config.prefabs.Count)];
            float offsetX = UnityEngine.Random.Range(config.randomRange.x, config.randomRange.y);
            float offsetZ = UnityEngine.Random.Range(config.randomRange.x, config.randomRange.y);
            Vector3 spawnPos = (Vector3)roadPosition + config.offset + new Vector3(offsetX, 0, offsetZ);
            Quaternion finalRotation = roadRotation * Quaternion.Euler(config.rotationOffset);

            Instantiate(prefab, spawnPos, finalRotation, transform);
        }

        public void FixRoad()
        {
            foreach (var position in fixRoadCandidates)
            {
                List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);
                Quaternion rotation = Quaternion.identity;

                if (neighbourDirections.Count == 1)
                {
                    Destroy(roadDictionary[position]);
                    if (neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Left))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Up))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
                }
                else if (neighbourDirections.Count == 2)
                {
                    if (neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Down)
                        || neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Left))
                    {
                        continue;
                    }

                    Destroy(roadDictionary[position]);
                    if (neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Right))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Left)
                        && neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
                }
                else if (neighbourDirections.Count == 3)
                {
                    Destroy(roadDictionary[position]);
                    if (neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Down)
                        && neighbourDirections.Contains(Direction.Left))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Down)
                            && neighbourDirections.Contains(Direction.Left)
                            && neighbourDirections.Contains(Direction.Up))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Left)
                        && neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Right))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(road3Way, position, rotation, transform);
                }
                else if (neighbourDirections.Count == 4)
                {
                    Destroy(roadDictionary[position]);
                    roadDictionary[position] = Instantiate(road4Way, position, rotation, transform);
                }
            }
        }

        public void Reset()
        {
            foreach (var item in roadDictionary.Values)
            {
                Destroy(item);
            }
            roadDictionary.Clear();
            fixRoadCandidates = new HashSet<Vector3Int>();
        }
    }
}