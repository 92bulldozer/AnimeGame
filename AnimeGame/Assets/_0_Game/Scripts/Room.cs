using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using EJ;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace AnimeGame
{
    public class Room : MonoBehaviour
    {
        public List<GameObject> frontWayPointList;
        public List<GameObject> backWayPointList;
       
        public CinemachineVirtualCamera vcam;

        public UnityEvent deActiveCallback;
        
        


        public virtual void Start()
        {
            //vcam.LookAt = GameManager.Instance.playerPresenter.transform;

        }

        public void SwapNavmeshCutSize()
        {
            NavmeshCut[] cuts = GetComponentsInChildren<NavmeshCut>();
            cuts.Length.Log(EColor.RED);
            foreach (var VARIABLE in cuts)
            {
                Vector2 swpVector2 = new Vector2(VARIABLE.rectangleSize.y, VARIABLE.rectangleSize.x);
                VARIABLE.rectangleSize = swpVector2;
            }
        }

        public virtual void EnterRoom()
        {
            vcam.LookAt = GameManager.Instance.playerPresenter.transform;
            GameManager.Instance.ChangeAmbientLight(true);
            GameManager.Instance.playerPresenter.ActiveLight();
        }

      

      

      
    }
}
