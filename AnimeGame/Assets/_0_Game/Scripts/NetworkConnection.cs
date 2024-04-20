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
        // Start is called before the first frame update
        void Start()
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
            if (obj.ConnectionState == LocalConnectionState.Stopping)
            {
                EditorApplication.isPlaying = false;
            }
        }

      

     

       
    }
}
