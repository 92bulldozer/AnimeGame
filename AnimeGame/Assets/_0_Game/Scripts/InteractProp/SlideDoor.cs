using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using Unity.VisualScripting;
using UnityEngine;

namespace AnimeGame
{

    public enum EVectorDirection
    {
        X=0,
        Y,
        Z
    }
    public class SlideDoor : InteractableObject
    {
        [Space(20)] [Header("DoorField")] [Space(10)] 
        public Transform door;
        public float moveValue;
        public bool isOpened;
        public string openDoorSfx;
        public string closeDoorSfx;
        public bool notRelative;
        public Vector3 openLocalPosition;
        public Vector3 closeLocalPosition;
        public EVectorDirection direction;

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
            if (!canInteract || isInteracted)
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
            if (!canInteract || isInteracted)
                return;
            
            isOpened = true;
            MasterAudio.PlaySound3DAtTransform(openDoorSfx, transform);
            isInteracted = true;

            if (!notRelative)
            {
                switch (direction)
                {
                    case EVectorDirection.X:
                        door.DOLocalMoveX(-moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            isInteracted = false;
                        });
                        break;
                    case EVectorDirection.Y:
                        break;
                    case EVectorDirection.Z:
                        door.DOLocalMoveZ(-moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            isInteracted = false;
                        });
                        break;
                }
            }
            else
            {
                door.DOLocalMove(openLocalPosition,1.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    isInteracted = false;
                });
            }
           
          
            UpdateReverseText(true);
            "Door 열기".Log();
            //sender.UpdateReverseText();
        }

        public void CloseDoor()
        {
            door.DOPause();
            isOpened = false;
            isInteracted = true;
            MasterAudio.PlaySound3DAtTransform(closeDoorSfx, transform);

            if (!notRelative)
            {
                switch (direction)
                {
                    case EVectorDirection.X:
                        door.DOLocalMoveX(moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            isInteracted = false;
                        });
                        break;
                    case EVectorDirection.Y:
                        break;
                    case EVectorDirection.Z:
                        door.DOLocalMoveZ(moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            isInteracted = false;
                        });
                        break;
                }
            }
            else
            {
                door.DOLocalMove(closeLocalPosition,1.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    isInteracted = false;
                });
            }
           

          
            
            UpdateReverseText(false);
            "Door 닫기".Log();
            //sender.UpdateReverseText();
            
        }
    }
}

