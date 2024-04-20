using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using FishNet.Object;
using FishNet.Object.Synchronizing;
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
        public readonly SyncVar<bool> isOpened = new SyncVar<bool>();
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


        [ServerRpc(RequireOwnership = false)]
        public void SetIsOpened(bool _isOpened) => isOpened.Value = _isOpened;
        // private void Update()
        // {
        //     $"HasAuthority={HasAuthority} canInteract.Value={canInteract.Value}  isInteracting.Value={isInteracting.Value} isOpened={isOpened.Value}".Log();
        // }


        public override void Interact()
        {
            if (!canInteract.Value || isInteracting.Value)
                return;
            
            base.Interact();
            
            if(isOpened.Value)
                CloseDoorServer();
            else
                OpenDoorServer();
            
                
        }

        public override void ShowInteractPanel()
        {
            base.ShowInteractPanel();
        }

        public override void HideInteractPanel()
        {
            base.HideInteractPanel();
        }

        [ServerRpc(RequireOwnership = false)]
        public void OpenDoorServer()
        {
            OpenDoorObserver();
        }

        
        [ObserversRpc(BufferLast = true)]
        public void OpenDoorObserver()
        {
            if (!canInteract.Value || isInteracting.Value)
                return;
            
            MasterAudio.PlaySound3DAtTransform(openDoorSfx, transform);
            if (HasAuthority)
            {
                SetIsOpened(true);
                SetIsInteracting(true);
            }
           
            "OpenDoor".Log();

            if (!notRelative)
            {
                switch (direction)
                {
                    case EVectorDirection.X:
                        door.DOLocalMoveX(-moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            if(HasAuthority)
                                SetIsInteracting(false);
                        });
                        break;
                    case EVectorDirection.Y:
                        break;
                    case EVectorDirection.Z:
                        door.DOLocalMoveZ(-moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            if(HasAuthority)
                                SetIsInteracting(false);
                        });
                        break;
                }
            }
            else
            {
                door.DOLocalMove(openLocalPosition,1.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    if (HasAuthority)
                        SetIsInteracting(false);
                });
            }
           
          
            UpdateReverseText(true);
            "Door 열기".Log();
            //sender.UpdateReverseText();
        }

        [ServerRpc(RequireOwnership = false)]
        public void CloseDoorServer()
        {
            CloseDoorObserver();
        }
        
        
        [ObserversRpc(BufferLast = true)]
        public void CloseDoorObserver()
        {
            door.DOPause();
            if (HasAuthority)
            {
                SetIsOpened(false);
                SetIsInteracting(true);
            }

            MasterAudio.PlaySound3DAtTransform(closeDoorSfx, transform);
            "CloseDoor".Log();

            if (!notRelative)
            {
                switch (direction)
                {
                    case EVectorDirection.X:
                        door.DOLocalMoveX(moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            if (HasAuthority)
                            {
                                SetIsInteracting(false);
                            }
                        });
                        break;
                    case EVectorDirection.Y:
                        break;
                    case EVectorDirection.Z:
                        door.DOLocalMoveZ(moveValue, 1.5f).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
                        {
                            if (HasAuthority)
                            {
                                SetIsInteracting(false);
                            }

                        });
                        break;
                }
            }
            else
            {
                door.DOLocalMove(closeLocalPosition,1.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    if (HasAuthority)
                    {
                        SetIsInteracting(false);
                    }
                });
            }
           

          
            
            UpdateReverseText(false);
            "Door 닫기".Log();
            //sender.UpdateReverseText();
            
        }
    }
}

