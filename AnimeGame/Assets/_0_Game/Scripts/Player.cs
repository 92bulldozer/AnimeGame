using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using EJ;
using UnityEngine;

namespace AnimeGame
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public bool canInteract;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy((gameObject));
            }

            Init();
        }
        
        public void Init()
        {
            canInteract=true;
        }

        public void ResetInteract()
        {
            canInteract = true;
            GameEventMessage.SendEvent("ResetInteract");
        }

      

    
    }
    
  
}


