using System;
using System.Collections;
using System.Collections.Generic;
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
        public ERoomType eRoomType;


        private void Start()
        {
            SpawnRoom();
        }

        public void SpawnRoom()
        {
            switch (eRoomType)
            {
                case ERoomType.BigClassRoom:
                    GameObject classRoomObj = Instantiate(roomData.bigClassRoomList[ Random.Range(0, 2)], transform);
                    room = classRoomObj.GetComponent<Room>();
                    room.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                    room.transform.localScale = Vector3.one;
                    break;
                case ERoomType.smallClassRoom:
                    GameObject smallRoomObj = Instantiate(roomData.bigClassRoomList[ Random.Range(0, 2)], transform);
                    room = smallRoomObj.GetComponent<Room>();
                    room.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                    room.transform.localScale = Vector3.one;
                    break;
              
            }
        }

        public void ExitRoom()
        {
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
