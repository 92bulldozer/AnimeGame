using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimeGame
{

    public enum ERoomType
    {
        BigClassRoom=0,
        smallClassRoom
    }
    public class RoomGenerator : MonoBehaviour
    {
        public RoomData roomData;
        public Room room;
        public GameObject frontRoomTrigger;
        public GameObject backRoomTrigger;
        public bool isSide;
      
        public ERoomType eRoomType;
        public bool isDebugMode;
        public int debugIdx;


        private void Start()
        {
            SpawnRoom();
        }

        public void SpawnRoom()
        {
            if (isDebugMode)
            {
                GameObject classRoomObj = Instantiate(roomData.bigClassRoomList[ debugIdx], transform);
                room = classRoomObj.GetComponent<Room>();
                room.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                room.transform.localScale = Vector3.one;
                room.frontWayPointList.Add(backRoomTrigger);
                room.backWayPointList.Add(frontRoomTrigger);
                
                if (isSide)
                {
                    room.SwapNavmeshCutSize();
                }
                return;
            }
            
            switch (eRoomType)
            {
                case ERoomType.BigClassRoom:
                    GameObject classRoomObj = Instantiate(roomData.bigClassRoomList[ Random.Range(0, roomData.bigClassRoomList.Count)], transform);
                    room = classRoomObj.GetComponent<Room>();
                    room.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                    room.transform.localScale = Vector3.one;
                    room.frontWayPointList.Add(backRoomTrigger);
                    room.backWayPointList.Add(frontRoomTrigger);
                    break;
                case ERoomType.smallClassRoom:
                    GameObject smallRoomObj = Instantiate(roomData.smallClassRoomList[ Random.Range(0, roomData.smallClassRoomList.Count)], transform);
                    room = smallRoomObj.GetComponent<Room>();
                    room.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                    room.transform.localScale = Vector3.one;
                    break;
              
            }

            if (isSide)
            {
                room.SwapNavmeshCutSize();
            }
        }

        public void ExitRoom()
        {
            GameManager.Instance.ChangeAmbientLight(false);
            GameManager.Instance.playerPresenter.DeActiveLight();
            try
            {
                
                room.deActiveCallback?.Invoke();
            }
            catch (Exception e)
            {
               
            }
        }

     

     
    }
}
