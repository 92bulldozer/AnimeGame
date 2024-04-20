using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

namespace AnimeGame
{
    public class FishNetSyncTestCharacter : NetworkBehaviour
    {
        public TextMeshProUGUI nameText;
        public readonly SyncVar<string> characterName = new SyncVar<string>();

        private void OnEnable()
        {
            characterName.OnChange+= CharacterNameOnOnChange;

        }

        private void OnDisable()
        {
            characterName.OnChange-= CharacterNameOnOnChange;

        }


        public override void OnStartClient()
        {
          
        }
        public override void OnStopClient()
        {
            
        }

        private void CharacterNameOnOnChange(string prev, string next, bool asserver)
        {
            nameText.text = next;
            $"OnChange = {next}".Log();
        }

        [ServerRpc]
        public void ChangeCharacterNameServer(string value)
        {
            characterName.Value = value;
            $"ServerRPC = {value}".Log();
        }

    

        // Update is called once per frame
        void Update()
        {
            if (IsOwner)
            {
                if(Input.GetKeyDown(KeyCode.K))
                {
                    ChangeCharacterNameServer(Owner.ClientId.ToString());
                }
            }
        }
    }
}
