using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimeGame
{
    public class WayPointManager : MonoBehaviour
    {

        public static WayPointManager Instance;

        public List<GameObject> wayPointList;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        public GameObject SelectRandomWayPoint()
        {
            return wayPointList[Random.Range(0, wayPointList.Count)];
        }
    }
}
