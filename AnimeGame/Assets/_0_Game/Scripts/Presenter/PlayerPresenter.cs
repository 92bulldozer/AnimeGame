using System;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine;
using EJ;
using MarkupAttributes;
using MoreMountains.Feedbacks;
using Rewired;
using RootMotion.Dynamics;
using UnityEngine;

public enum EBody
{
    Hip=0,
    LLeg=1,
    RLeg=4,
    Chest=7,
    Head=8,
    LArm=9,
    RArm=12
}

public enum KnockBackDirection
{
    FRONT=0,
    BACK,
    LEFT,
    RIGHT,
    INPLACE
}



namespace AnimeGame
{
    [RequireComponent(typeof(PlayerInteract))]
    public class PlayerPresenter : AnimeBehaviour
    {
        public static PlayerPresenter Instance;

        [TabScope("Tab Scope", "Rewired|Component|MMF|Sfx|HangTransform", box: true,20)]
  
        [Tab("./Rewired")]
        public int playerID;
        public Player player;
        
        [Tab("../Component")]
        public PuppetMaster puppetMaster;
        public List<MuscleCollisionBroadcaster> ragDollCollisionList;
        public GameObject flashLight;
        public GameObject virtualCamera;
        public PlayerInteract playerInteract;
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rb;
        public Animator _animator;
        private Camera _mainCamera;

        [Tab("../MMF")]
        public MMF_Player MMF_CameraShake;
        
        [Tab("../Sfx")]
        public string footStepSfx;

        [Tab("../HangTransform")]
        public Transform LHandTarget;
        public Transform RHandTarget;
        public Transform chestTarget;
        public Transform headTarget;
        
        [Box("Field",true,10)]
        public bool isFlashOn;
        public bool isRagDoll;
        public bool isAlive { get; set; }
        private Sequence pickupSequence;

       

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
        

     

        private void Update()
        {
            playerInteract.FindInteractObject();

            if (player.GetButtonDown("Interact"))
            {
                Interact();
            }
            
            if (player.GetButtonDown("Flash"))
            {
                FlashLightToggle();
            }
            
            
            
      


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //$"PlayerPresenterAlpha1".Log();
                //AttachTo(null);
                //GetDamaged(null,KnockBackDirection.LEFT);
                //GetDamaged(null,true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //$"PlayerPresenterAlpha2".Log();
                //DetachTo();
                //GetDamaged(null,KnockBackDirection.RIGHT);
                //DeActiveRagDoll();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //ActiveRagDoll();
                //GetDamaged(null,KnockBackDirection.BACK);
            }
            
         
            
            // if (moveDirection != Vector3.zero)
            // {
            //     Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
            //     _rb.rotation = Quaternion.Slerp(_rb.rotation,lookRotation,rotationSpeed*Time.deltaTime);
            //     
            // }
            // else
            // {
            //     _rb.angularVelocity=Vector3.zero;
            //     _rb.velocity  = Vector3.zero;
            // }

            // float characterAnimationSpeed = _rb.velocity.magnitude.Remap(0, 2, 0, 1);
            // _animator.SetFloat("MoveSpeed",characterAnimationSpeed,animationSmoothTime,Time.deltaTime);
            
        }

    
        


        public void Init()
        {
            isAlive = true;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            _capsuleCollider = GetComponent<CapsuleCollider>();
            player = ReInput.players.GetPlayer(playerID);
            playerInteract = GetComponent<PlayerInteract>();
            playerInteract.Init();
            pickupSequence = DOTween.Sequence().SetAutoKill(false).Pause().OnPlay(()=>
            {
                "DisableInput".Log();
                DisableInput();
            }).SetDelay(0.8f).OnComplete(() =>
            {
                "EnableInput".Log();
                EnableInput();
            });
            //player.controllers.maps.GetMap(ControllerType.Keyboard, 0, 0).enabled=false;


            DOVirtual.DelayedCall(0.5f, () =>
            {
                ragDollCollisionList = puppetMaster.transform.GetComponentsInChildren<MuscleCollisionBroadcaster>()
                    .ToList();

            });

        }
        

        public void ResetInteract()
        {
            //canInteract = true;
            GameEventMessage.SendEvent("ResetInteract");
        }



        public void FlashLightToggle()
        {
            if (!isFlashOn)
            {
                isFlashOn = true;
                flashLight.SetActive(true);
                MasterAudio.PlaySound3DAtTransform("FlashLightOn",transform);
            }
            else
            {
                isFlashOn = false;
                flashLight.SetActive(false);
                MasterAudio.PlaySound3DAtTransform("FlashLightOff",transform);
            }
          
        }

        #region Input

      
        
        public void OnAttack()
        {
            Debug.Log("Attack");
        }

        #endregion




        public void FootStep()
        {
            MasterAudio.PlaySound3DAtTransform(footStepSfx, transform);
        }

        public void ActiveRagDoll()
        {
            isAlive = false;
            _rb.Sleep();
            _animator.enabled = false;
            _capsuleCollider.enabled = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            isRagDoll = true;
            puppetMaster.Kill();
           


            MasterAudio.PlaySound3DAtTransform("FemaleScream", transform);
            "RagDollActive".Log();
        }


        public void DeActiveRagDoll()
        {
            isRagDoll = false;
            _animator.enabled = true;
            _capsuleCollider.enabled = true;
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
            _rb.WakeUp();
           


            "RagDollDeActive".Log();
        }

        

        public void Interact()
        {
            if (playerInteract.currentInteractableObject == null || !playerInteract.canInteract)
                return;
            
            playerInteract.currentInteractableObject.Interact();   
        }

      

        public void ChangeVirtualCamera(GameObject _virtualCamera)
        {
          
            virtualCamera.SetActive(false);
            _virtualCamera.SetActive(true);
            virtualCamera = _virtualCamera;
            
        }



        public void DisableInput()
        {
            GameManager.Instance.DisableInput();
        }
        
        public void EnableInput()
        {
            GameManager.Instance.EnableInput();
        }

        public void PickUp()
        {
            _animator.SetTrigger("PickUp");
            pickupSequence.Restart();
        }

        #region Hit_Blood

      

        public void AttachTo(Transform attachTransform)
        {
            ActiveRagDoll();
            _rb.rotation *= Quaternion.Euler(0,180,0);
            headTarget.gameObject.SetActive(true);
            headTarget.parent = attachTransform;
            headTarget.DOLocalMove(Vector3.zero, 1);

            //chestTarget.DOLocalMove(Vector3.zero, 1);
        }
        
        public void DetachTo()
        {
            headTarget.gameObject.SetActive(false);
        }

        public void GetDamaged(Transform enemyTransform,KnockBackDirection direction)
        {
            ActiveRagDoll();
            
            float force = 30000;
            EBody hitBodyName = EBody.Chest;

            switch (direction)
            {
                case KnockBackDirection.FRONT:
                    ragDollCollisionList[(int)hitBodyName].Hit(5,-enemyTransform.forward * force,ragDollCollisionList[0].transform.localPosition-enemyTransform.right*0.5f);
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.BACK:
                    ragDollCollisionList[(int)hitBodyName].Hit(5,enemyTransform.forward * force,ragDollCollisionList[0].transform.localPosition-enemyTransform.right*0.5f);
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.LEFT:
                    ragDollCollisionList[(int)hitBodyName].Hit(5,-enemyTransform.right * force,ragDollCollisionList[0].transform.localPosition-enemyTransform.right*0.5f);
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.RIGHT:
                    ragDollCollisionList[(int)hitBodyName].Hit(5,enemyTransform.right * force,ragDollCollisionList[0].transform.localPosition+enemyTransform.right*0.5f);
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.INPLACE:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            
            MMF_CameraShake.PlayFeedbacks();
           
        }
        
        
        public void GetDamagedOnlyBlood(Transform enemyTransform,KnockBackDirection direction)
        {
            
            EBody hitBodyName = EBody.Chest;

            switch (direction)
            {
                case KnockBackDirection.FRONT:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.BACK:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.LEFT:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.RIGHT:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                case KnockBackDirection.INPLACE:
                    VfxPresenter.Instance.PlayBloodVfx( ragDollCollisionList[(int)hitBodyName].transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            
            MMF_CameraShake.PlayFeedbacks();
           
        }

        

        #endregion
        
        
        
        
    
    }
}


