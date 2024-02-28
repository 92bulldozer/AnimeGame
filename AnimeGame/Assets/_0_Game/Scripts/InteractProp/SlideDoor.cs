using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using Unity.VisualScripting;
using UnityEngine;

namespace AnimeGame
{
    public class SlideDoor : InteractableObject
    {
        public Transform door;
        public float moveValue;
        public bool isOpened;
        public string openDoorSfx;
        public string closeDoorSfx;
        public InteractSender sender;
        
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Interact()
        {
            if (!canInteract)
                return;
            
            base.Interact();
            if(isOpened)
                CloseDoor();
            else
                OpenDoor();
            
                
        }

        public override void ShowInteractPanel()
        {
            base.ShowInteractPanel();
        }

        public override void HideInteractPanel()
        {
            base.HideInteractPanel();
        }

        public void OpenDoor()
        {
            isOpened = true;
            MasterAudio.PlaySound3DAtTransform(openDoorSfx, transform);
            door.DOLocalMoveX(-moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
            {
                canInteract = true;
            });
            "Door 열기".Log();
            sender.UpdateReverseText();
        }

        public void CloseDoor()
        {
            isOpened = false;
            MasterAudio.PlaySound3DAtTransform(closeDoorSfx, transform);
            door.DOLocalMoveX(moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
            {
                canInteract = true;
            });
            "Door 닫기".Log();
            sender.UpdateReverseText();
            
        }
    }
}

