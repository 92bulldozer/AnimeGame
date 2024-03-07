using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine;
using EJ;
using MoreMountains.Feedbacks;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
    RIGHT
}



namespace AnimeGame
{
    public class PlayerPresenter : MonoBehaviour
    {
        public static PlayerPresenter Instance;

        [Header("Field")] [Space(10)] 
        public bool canInteract = false;
        public bool isFlashOn;
      
        public bool isRagDoll;
        public bool isAlive { get; set; }


        [Header("Component")] [Space(10)] 
        public PuppetMaster puppetMaster;
        public List<MuscleCollisionBroadcaster> ragDollCollisionList;
        public GameObject flashLight;
        public InteractableObject interactableObject;
        public GameObject virtualCamera;
        public PlayerInput playerInput;



        private Camera _mainCamera;
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rb;
        public Animator _animator;

        [Header("MMF")] [Space(10)] 
        public MMF_Player MMF_CameraShake;
        [Header("Sfx")] [Space(10)] 
        public string footStepSfx;

       

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
            if(Input.GetKeyDown(KeyCode.F))
                FlashLightToggle();



            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                
                GetDamaged(null,KnockBackDirection.LEFT);
                //GetDamaged(null,true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GetDamaged(null,KnockBackDirection.RIGHT);
                //DeActiveRagDoll();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //ActiveRagDoll();
                GetDamaged(null,KnockBackDirection.BACK);
            }
            
            if(Input.GetKeyDown(KeyCode.E))
                Interact();
            
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

        private void FixedUpdate()
        {
            if (isRagDoll)
                return;
            
          
            
        }

        


        public void Init()
        {
            isAlive = true;
            canInteract=true;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            _capsuleCollider = GetComponent<CapsuleCollider>();
           

            playerInput = GetComponent<PlayerInput>();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                ragDollCollisionList = puppetMaster.transform.GetComponentsInChildren<MuscleCollisionBroadcaster>()
                    .ToList();

            });

        }
        

        public void ResetInteract()
        {
            canInteract = true;
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
            if (interactableObject == null)
                return;
            
            interactableObject.Interact();   
        }

        public void SetInteractObject(InteractableObject _interactableObject)
        {
            interactableObject = _interactableObject;
        }

        public void ChangeVirtualCamera(GameObject _virtualCamera)
        {
            virtualCamera.SetActive(false);
            _virtualCamera.SetActive(true);
            virtualCamera = _virtualCamera;
        }



        public void DisableInput()
        {
            playerInput.enabled = false;
        }
        
        public void EnableInput()
        {
            playerInput.enabled = true;
        }

        #region Hit_Blood

        

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            
            MMF_CameraShake.PlayFeedbacks();
           
        }

        

        #endregion
        
        
        
        
    
    }
}


