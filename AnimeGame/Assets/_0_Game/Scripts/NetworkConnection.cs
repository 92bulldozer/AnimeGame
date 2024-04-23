using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEditor;
using UnityEngine;

namespace AnimeGame
{
    public enum ConnectionType
    {
        Host=0,
        Client
    }

    public class NetworkConnection : MonoBehaviour
    {
        public ConnectionType connectionType;

        public bool isDebugMode;
        // Start is called before the first frame update
        void Start()
        {
            if (!isDebugMode)
            {
                if (connectionType == ConnectionType.Host)
                {
                    InstanceFinder.ServerManager.StartConnection();
                    InstanceFinder.ClientManager.StartConnection();
                }
                else
                {
                    InstanceFinder.ClientManager.StartConnection();
                
                }
            }
            else
            {
                
            }
           
        }

        private void Update()
        {
            if (!isDebugMode)
                return;
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               StartHost();
               isDebugMode = false;

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
               StartClient();
               isDebugMode = false;
            }
        }

        public void StartHost()
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        }

        public void StartClient()
        {
            InstanceFinder.ClientManager.StartConnection();

        }

        private void OnEnable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState+= ClientManagerOnOnClientConnectionState;
        }
        
        private void OnDisable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState-= ClientManagerOnOnClientConnectionState;
        }

        private void ClientManagerOnOnClientConnectionState(ClientConnectionStateArgs obj)
        {
#if UNITY_EDITOR
            
            if (obj.ConnectionState == LocalConnectionState.Stopping)
            {
                EditorApplication.isPlaying = false;
            }
#endif

        }

      

     

       
    }
}
