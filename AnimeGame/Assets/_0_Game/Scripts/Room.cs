using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace AnimeGame
{
    public class Room : MonoBehaviour
    {
        public CinemachineVirtualCamera vcam;

        public UnityEvent deActiveCallback;


        private void Start()
        {
            vcam.LookAt = PlayerPresenter.Instance.transform;

        }

        public void EnterRoom()
        {
            GameManager.Instance.ChangeAmbientLight(true);
            PlayerPresenter.Instance.ActiveLight();
        }

      
    }
}
