using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimeGame
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "SO/Room Data")]
    public class RoomData : ScriptableObject
    {
        public List<GameObject> bigClassRoomList;
        public List<GameObject> smallClassRoomList;
    }
}
