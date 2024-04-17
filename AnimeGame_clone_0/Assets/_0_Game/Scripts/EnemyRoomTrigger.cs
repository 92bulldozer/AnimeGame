using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EJ;
using UnityEngine;

namespace AnimeGame
{
    public enum EEnemyRoomTriggerDirection
    {
        FRONT = 0,
        BACK
    }

    public class EnemyRoomTrigger : MonoBehaviour
    {
        public EEnemyRoomTriggerDirection direction;
        public RoomGenerator roomGenerator;


        public void EnterRoom(EnemyPresenter enemyPresenter)
        {


            
            switch (direction)
            {
                case EEnemyRoomTriggerDirection.FRONT:
                    enemyPresenter.wayPointList = roomGenerator.room.frontWayPointList.ToList();

                    break;
                case EEnemyRoomTriggerDirection.BACK:
                    enemyPresenter.wayPointList = roomGenerator.room.backWayPointList.ToList();

                    break;
            }
        }
    }
}