using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimeGame
{
    public class CameraTimeStopTrigger : MonoBehaviour
    {
        public ChangeCameraTrigger changeCameraTrigger;
        public bool isTimeStop;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                changeCameraTrigger.useTimeStop = isTimeStop;

            }
        }
    }
}
