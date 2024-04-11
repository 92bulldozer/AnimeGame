using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
