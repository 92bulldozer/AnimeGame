using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimeGame
{

    public enum ECameraPositionType
    {
        Floor=0,
        Stair
    }
    public class CameraTriggerManager : MonoBehaviour
    {
        public static CameraTriggerManager Instance;

        public List<ChangeCameraTrigger> floorTriggerList;
        public List<ChangeCameraTrigger> stairTriggerList;



        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetFloorTimeStop()
        {
            foreach (var VARIABLE in floorTriggerList)
            {
                VARIABLE.useTimeStop = true;
            }
            foreach (var VARIABLE in stairTriggerList)
            {
                VARIABLE.useTimeStop = false;
            }
        }

        
        public void SetStairTimeStop()
        {
            foreach (var VARIABLE in floorTriggerList)
            {
                VARIABLE.useTimeStop = false;
            }
            foreach (var VARIABLE in stairTriggerList)
            {
                VARIABLE.useTimeStop = true;
            }
        }
    }
}
