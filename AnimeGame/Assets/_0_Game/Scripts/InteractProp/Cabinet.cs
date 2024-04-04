using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using Rewired;
using RootMotion.Dynamics;
using UnityEngine;

namespace AnimeGame
{
    public class Cabinet : InteractableObject
    {
        public bool isHided;
        public string hideSfx;
        public string comeOutSfx;
        public Transform characterComeOutTransform;
        private Player _player;
        public Transform doorTransform;
        private BoxCollider doorCollider;
        
        public override void Awake()
        {
            base.Awake();
            _player = ReInput.players.GetPlayer(0);
            doorCollider = doorTransform.GetComponent<BoxCollider>();
        }

        public override void Start()
        {
            base.Start();
        }

       

        public override void Interact()
        {
            if (!canInteract || isInteracted)
                return;
            
            base.Interact();
            
            if(isHided)
                ComeOut();
            else
                Hide();
            
                
        }

        public override void ShowInteractPanel()
        {
            base.ShowInteractPanel();
        }

        public override void HideInteractPanel()
        {
            base.HideInteractPanel();
        }

        public void Hide()
        {
            if (!canInteract || isInteracted)
                return;
            
            isHided = true;
            MasterAudio.PlaySound3DAtTransform(hideSfx, transform);
            isInteracted = true;
            transform.DOShakeRotation(0.5f, new Vector3(2, 0, 3), 30)
                .SetEase(Ease.OutQuad)
                .OnComplete(()=>
                {
                    isInteracted = false;
                    CheckComeOutInput().Forget();

                });

            GameManager.Instance.playerPresenter.gameObject.SetActive(false);
            doorTransform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
           
            UpdateReverseText(true);
            "Hide".Log();
            //sender.UpdateReverseText();
        }

        public void ComeOut()
        {
            isHided = false;
            //isInteracted = true;
            MasterAudio.PlaySound3DAtTransform(comeOutSfx, transform);

            
            PlayerPresenter.Instance.transform.SetPositionAndRotation(characterComeOutTransform.position,characterComeOutTransform.rotation);
            GameManager.Instance.playerPresenter.gameObject.SetActive(true);
            doorCollider.enabled = false;
            doorTransform.DOLocalRotate(new Vector3(0, -120, 0), 0.1f).SetEase(Ease.OutQuad)
                .OnComplete(() => doorCollider.enabled = true);
            UpdateReverseText(false);
            "ComeOut".Log();
            //sender.UpdateReverseText();
            
        }

        public async UniTaskVoid CheckComeOutInput()
        {
            while (true)
            {
                if (_player.GetButtonDown("Interact"))
                {
                    ComeOut();
                    break;
                }
                await UniTask.Yield();

            }
            "Interact 기모찌".Log(EColor.RED);
        }

       
    }
    
}
