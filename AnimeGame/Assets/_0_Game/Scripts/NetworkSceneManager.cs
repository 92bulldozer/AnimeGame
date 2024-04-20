using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

namespace AnimeGame
{
    public class NetworkSceneManager : MonoBehaviour
    {
        public static NetworkSceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (InstanceFinder.IsServerStarted)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    NetworkLoadScene("NetworkSceneTest1");
                    NetworkUnLoadScene("NetworkSceneTest2");
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    NetworkLoadScene("NetworkSceneTest2");
                    NetworkUnLoadScene("NetworkSceneTest1");
                }
            }
        }

        public void NetworkLoadScene(string sceneName)
        {
            if (!InstanceFinder.IsServerStarted)
                return;

            SceneLoadData sld = new SceneLoadData(sceneName);
            InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        }
    
        public void NetworkUnLoadScene(string sceneName)
        {
            if (!InstanceFinder.IsServerStarted)
                return;

            SceneUnloadData sud = new SceneUnloadData(sceneName);
            InstanceFinder.SceneManager.UnloadGlobalScenes(sud);
        }
    }
}
