using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

namespace SVS
{
    public class RoadHelper : MonoBehaviour
    {
        public Action finishedCoroutine;
        [SerializeField] GameObject roadStrainght;
        [SerializeField] GameObject roadCorner;
        [SerializeField] GameObject road3Way;
        [SerializeField] GameObject road4Way;
        [SerializeField] GameObject roadEnd;

        public float animationTime = 0.01f;

        Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
        HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

        public List<Vector3Int> GetRoadPositions()
        {
            return roadDictionary.Keys.ToList();
        }

        public IEnumerator PlaceStreetPositions(UnityEngine.Vector3 startPosition, Vector3Int direction, int lenght)
        {
            var rotation = UnityEngine.Quaternion.identity;
            if(direction.x == 0)
            {
                rotation = UnityEngine.Quaternion.Euler(0,90,0);
            }

            for (int i = 0; i < lenght; i++)
            {
                var position = Vector3Int.RoundToInt(startPosition + direction * i);
                if(roadDictionary.ContainsKey(position))
                {
                    continue;
                }
                var road = Instantiate(roadStrainght, position, rotation, transform);
                //road.AddComponent<FallTween>();
                roadDictionary.Add(position, road);

                if(i==0 || i == lenght -1)
                {
                    fixRoadCandidates.Add(position);
                }
                yield return new WaitForSeconds(animationTime);
            }
            finishedCoroutine?.Invoke();
        }

        public void FixRoad()
        {
            foreach(var position in fixRoadCandidates)
            {
                List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);
                UnityEngine.Quaternion rotation = UnityEngine.Quaternion.identity;

                if(neighbourDirections.Count == 1)
                {
                    Destroy(roadDictionary[position]);
                    if(neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0,90,0);
                    }
                    else if(neighbourDirections.Contains(Direction.Left))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0,180,0);
                    }
                    else if(neighbourDirections.Contains(Direction.Up))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0,-90,0);
                    }

                    roadDictionary[position] = Instantiate(roadEnd, position , rotation, transform);

                }
                else if (neighbourDirections.Count == 2)
                {
                    if(neighbourDirections.Contains(Direction.Up) 
                        && neighbourDirections.Contains(Direction.Down)
                        || neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Left))
                        {
                            continue;
                        }
                        
                        Destroy(roadDictionary[position]);
                        if(neighbourDirections.Contains(Direction.Up) 
                            && neighbourDirections.Contains(Direction.Right) )
                        {
                            rotation = UnityEngine.Quaternion.Euler(0,90,0);
                        }
                        else if(neighbourDirections.Contains(Direction.Right) 
                            && neighbourDirections.Contains(Direction.Down))
                        {
                            rotation = UnityEngine.Quaternion.Euler(0,180,0);
                        }
                        else if(neighbourDirections.Contains(Direction.Left)
                            && neighbourDirections.Contains(Direction.Down))
                        {
                            rotation = UnityEngine.Quaternion.Euler(0,-90,0);
                        }

                        roadDictionary[position] = Instantiate(roadCorner, position , rotation, transform);
                }
                else if(neighbourDirections.Count == 3)
                {
                        Destroy(roadDictionary[position]);
                        if(neighbourDirections.Contains(Direction.Right) 
                            && neighbourDirections.Contains(Direction.Down)
                            && neighbourDirections.Contains(Direction.Left) )
                        {
                            rotation = UnityEngine.Quaternion.Euler(0,90,0);
                        }
                        else if(neighbourDirections.Contains(Direction.Down) 
                                && neighbourDirections.Contains(Direction.Left)
                                && neighbourDirections.Contains(Direction.Up))
                        {
                            rotation = UnityEngine.Quaternion.Euler(0,180,0);

                        }
                        else if(neighbourDirections.Contains(Direction.Left)
                        && neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Right))
                        {
                                {
                                    rotation = UnityEngine.Quaternion.Euler(0,-90,0);
                                }
                        }

                        roadDictionary[position] = Instantiate(road3Way, position , rotation, transform);

                }
                else
                {
                    Destroy(roadDictionary[position]);
                    roadDictionary[position] = Instantiate(road4Way, position, rotation,transform);
                }
            }
        }

        public void Reset()
        {
            foreach(var item in roadDictionary.Values)
            {
                Destroy(item);
            }
            roadDictionary.Clear();
            fixRoadCandidates = new HashSet<Vector3Int>();
        }
    }
}