using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimeGame
{
    public class RestRoom : Room
    {
        public override void Start()
        {
            base.Start();
        }

        public override void EnterRoom()
        {
            base.EnterRoom();
        }

        public void ExitRoom()
        {
            GameManager.Instance.ChangeAmbientLight(false);
            PlayerPresenter.Instance.DeActiveLight();
            try
            {
                
                deActiveCallback?.Invoke();
            }
            catch (Exception e)
            {
               
            }
        }
    }   
}
