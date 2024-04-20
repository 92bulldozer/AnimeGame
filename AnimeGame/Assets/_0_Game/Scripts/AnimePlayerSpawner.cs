using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimeGame
{
    public class AnimePlayerSpawner : NetworkBehaviour
    {
        public List<GameObject> playerPrefabList;
        public int playerIdx;


        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner)
                enabled = false;
        }

        private void Update()
        {
            if (!IsOwner)
                return;
            
            if(Input.GetKeyDown(KeyCode.Alpha3))
                SpawnPlayer();
        }

      

        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayer()
        {
            GameObject playerObj = Instantiate(playerPrefabList[1],
                GameManager.Instance.playerSpawnTransformList[Random.Range(0,GameManager.Instance.playerSpawnTransformList.Count)].position,
                Quaternion.identity);
            
            ServerManager.Spawn(playerObj,Owner);
            $"SpawnPlayer {playerIdx} ".Log();
           

        }
    }
}
