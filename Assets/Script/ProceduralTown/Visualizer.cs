using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SVS
{
    public class Visualizer : MonoBehaviour
    {

        [Header("Enemigos")]
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] Transform parentEnemies;
        [SerializeField] int enemyCount = 5;
        private List<GameObject> _enemyInstances = new List<GameObject>(); // Lista de control de instancias de enemigos

        [Header("Kits Médicos")]
        [SerializeField] GameObject medicalKitPrefab;
        [SerializeField] int medicalKitCount = 3;
        [SerializeField] Transform parentMedicalKits;
        private List<GameObject> _medicalKitInstances = new List<GameObject>();

        [Header("SecondPlayer")]
        [SerializeField] GameObject finishMarker;
        private GameObject _groundPlaneInstance; //instancia de Plane

        [Header("Ground")]
        [SerializeField] GameObject groundPlane;
        private GameObject _finishMarkerInstance; //instancia de marca final

        [Header("Map")]
        [SerializeField] LSystemGenerator lSystem;
        List<Vector3> positions = new List<Vector3>();

        [SerializeField] RoadHelper roadHelper;
        [SerializeField] StructureHelper structureHelper;

        private int roadLenght = 8;
        private int lenght = 8;
        [SerializeField][Range(0f, 90f)] private float angle = 90f;

        private bool waitingForTheRoad = false;

        public int Lenght
        {
            get
            {
                return lenght > 0 ? lenght : 1;
            }
            set => lenght = value;
        }
        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }

        private void Start()
        {
            roadHelper.finishedCoroutine += () => waitingForTheRoad = false;
            CreateTown();
        }

        public void CreateTown()
        {
            ClearEnemies();
            ClearMedicalKits();

            lenght = roadLenght;

            roadHelper.Reset();
            structureHelper.Reset();

            var sequence = lSystem.GenerateSentence();
            StartCoroutine(VisualizeSequence(sequence));

        }

        private IEnumerator VisualizeSequence(string sequence)
        {
            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            Vector3 currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;


            foreach (var letter in sequence)
            {
                if (waitingForTheRoad)
                {
                    yield return new WaitForEndOfFrame();
                }

                EncodingLetters encoding = (EncodingLetters)letter;

                switch (encoding)
                {
                    case EncodingLetters.unknown:
                        break;
                    case EncodingLetters.save:
                        savePoints.Push(new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            lenght = Lenght
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Lenght = agentParameter.lenght;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in Stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * lenght;
                        StartCoroutine(roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), lenght));

                        waitingForTheRoad = true;
                        yield return new WaitForEndOfFrame();

                        Lenght -= 2;
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;

                }

            }
            yield return new WaitForSeconds(0.1f);
            roadHelper.FixRoad();

            //SpawnPlayerAtStart();
            SpawnFinishZoneAtEnd();
            SpawnGroundPlane();
            SpawnEnemies(roadHelper.GetRoadPositions());
            SpawnMedicalKits(roadHelper.GetRoadPositions());

            yield return new WaitForSeconds(0.8f);
            StartCoroutine(structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoadPositions()));

        }

        private void SpawnFinishZoneAtEnd()
        {

            // Obtiene la lista de posiciones de la carretera
            var roadPositions = roadHelper.GetRoadPositions();
            if (roadPositions == null || roadPositions.Count == 0)
            {
                Debug.LogError("No hay posiciones de carretera para colocar la meta.");
                return;
            }

            // Toma la última posición de la lista
            Vector3Int last = roadPositions[roadPositions.Count - 1];
            Vector3 spawnPos = new Vector3(last.x, last.y, last.z);

            // Instancia solo una vez
            if (_finishMarkerInstance == null)
            {
                _finishMarkerInstance = Instantiate(finishMarker, spawnPos, Quaternion.identity);
            }
            else
            {
                _finishMarkerInstance.transform.position = spawnPos;
            }
        }

        private void SpawnGroundPlane()
        {
            if (groundPlane == null)
            {
                Debug.LogWarning("No has asignado groundPlane en el Inspector.");
                return;
            }

            Vector3 groundPosition = new Vector3(0, 0, 0);

            if (_groundPlaneInstance == null)
            {
                _groundPlaneInstance = Instantiate(groundPlane, groundPosition, Quaternion.identity);
            }
            else
            {
                _groundPlaneInstance.transform.position = groundPosition;
            }
        }


        private void SpawnEnemies(List<Vector3Int> roadPositions)
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("No has asignado el enemyPrefab en el Inspector.");
                return;
            }

            var spawnPositions = GetEnemySpawnPositions(roadPositions, enemyCount);

            foreach (var pos in spawnPositions)
            {
                Vector3 worldPos = new Vector3(pos.x, pos.y + 0.02f, pos.z);
                var enemyIns = Instantiate(enemyPrefab, worldPos, Quaternion.identity);
                enemyIns.transform.SetParent(parentEnemies); //guardamos clones en el padre

                // Guarda la instancia en la lista
                _enemyInstances.Add(enemyIns);

            }

        }

        //genera las posiciones de los enemies
        private List<Vector3> GetEnemySpawnPositions(List<Vector3Int> roadPositions, int count)
        {
            var list = new List<Vector3>();
            var rnd = new System.Random();
            int attempts = 0;

            while (list.Count < count && attempts < count * 10)
            {
                attempts++;
                // Elige una carretera al azar
                var roadPos = roadPositions[rnd.Next(roadPositions.Count)];
                // Desplázate lateralmente 2 a 4 unidades
                float offsetX = (float)(rnd.NextDouble() * 4 - 2);
                float offsetZ = (float)(rnd.NextDouble() * 4 - 2);
                Vector3 spawn = new Vector3(roadPos.x + offsetX, roadPos.y, roadPos.z + offsetZ);

                // Comprueba que no esté demasiado cerca de otro enemigo
                if (!list.Exists(p => Vector3.Distance(p, spawn) < 2f)) // Distancia mínima de 2 unidades
                    list.Add(spawn);
            }
            return list;
        }

        private void ClearEnemies()
        {
            foreach (var e in _enemyInstances)
            {
                if (e != null)
                    Destroy(e);
            }
            _enemyInstances.Clear();
        }

        private void SpawnMedicalKits(List<Vector3Int> roadPositions)
        {
            if (medicalKitPrefab == null)
            {
                Debug.LogWarning("No has asignado el medicalKitPrefab en el Inspector.");
                return;
            }

            var spawnPositions = GetMedicalKitSpawnPositions(roadPositions, medicalKitCount);

            foreach (var pos in spawnPositions)
            {
                Vector3 worldPos = new Vector3(pos.x, pos.y + 0.1f, pos.z);
                var kit = Instantiate(medicalKitPrefab, worldPos, Quaternion.identity);
                if (parentMedicalKits != null)
                    kit.transform.SetParent(parentMedicalKits);
                _medicalKitInstances.Add(kit);
            }
        }

        private List<Vector3> GetMedicalKitSpawnPositions(List<Vector3Int> roadPositions, int count)
        {
            var list = new List<Vector3>();
            var rnd = new System.Random();
            int attempts = 0;

            while (list.Count < count && attempts < count * 10)
            {
                attempts++;
                var roadPos = roadPositions[rnd.Next(roadPositions.Count)];
                float offsetX = (float)(rnd.NextDouble() * 4 - 2);
                float offsetZ = (float)(rnd.NextDouble() * 4 - 2);
                Vector3 spawn = new Vector3(roadPos.x + offsetX, roadPos.y, roadPos.z + offsetZ);

                if (!list.Exists(p => Vector3.Distance(p, spawn) < 2f))
                    list.Add(spawn);
            }
            return list;
        }

        private void ClearMedicalKits()
        {
            foreach (var kit in _medicalKitInstances)
            {
                if (kit != null)
                    Destroy(kit);
            }
            _medicalKitInstances.Clear();
        }

    }

}