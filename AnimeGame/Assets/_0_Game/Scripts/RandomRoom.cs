using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace AnimeGame
{
    public class RandomRoom : MonoBehaviour
    {
        public CinemachineVirtualCamera vcam;

        private void Awake()
        {
        }

        private void Start()
        {
            vcam.LookAt = PlayerPresenter.Instance.transform;

        }
    }
}
