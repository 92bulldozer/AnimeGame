using System;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine;
using ECM.Controllers;
using EJ;
using FishNet.Component.Animating;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
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
    public class PlayerPresenter : AnimeNetworkBehaviour
    {
        //public static PlayerPresenter Instance;

        [TabScope("Tab Scope", "Rewired|Component|Sfx|HangTransform", box: true,20)]
  
        [Tab("./Rewired")]
        public int playerID;
        public Player player;
        private NetworkManager _networkManager;
        private BaseCharacterController _baseCharacterController;
        [Tab("../Component")]
        public PuppetMaster puppetMaster;
        public List<MuscleCollisionBroadcaster> ragDollCollisionList;
        public GameObject flashLight;
        public GameObject virtualCamera;
        public PlayerInteract playerInteract;
        public AnimeCharacterController ac;
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rb;
        public Animator _animator;
        private NetworkAnimator _networkAnimator;
        private Camera _mainCamera;
        public GameObject characterSpotLight;
        public GameObject characterPointLight;
        

        
        [Tab("../Sfx")]
        public string footStepSfx;

        [Tab("../HangTransform")]
        public Transform LHandTarget;
        public Transform RHandTarget;
        public Transform chestTarget;
        public Transform headTarget;
        public Transform pickUpTransform;
        
        [Box("Field",true,10)]
        public bool isFlashOn;
        public bool isRagDoll;
        public bool isCrouch;
        public bool isAlive { get; set; }
        private Sequence pickupSequence;

        //public readonly SyncVar<int> NHealth = new SyncVar<int>();
        public readonly SyncVar<int> NHealth = new SyncVar<int>( );
        public readonly SyncVar<string> charactername = new SyncVar<string>();
        public GameObject spherePrefab;
        public GameObject spawnedObject;

       

        private void Awake()
        {
            // if (Instance == null)
            //     Instance = this;
            // else
            // {
            //     Destroy((gameObject));
            // }

            Init();
        }

       
        
        
        
        [ServerRpc]
        private void SetCharacterNameServer(string _name)
        {
            // charactername.Value = _name;
            // gameObject.name = _name;
            SetCharacterNameObserver(_name);
            $"SetCharacterNameServerRPC {_name}".Log();
        }
        
        [ObserversRpc]
        private void SetCharacterNameObserver(string _name)
        {
            charactername.Value = _name;
            gameObject.name = _name;
            $"{gameObject.name} SetHealthObserverRPC".Log();
        }
        
        private void CharacternameOnOnChange(string prev, string next, bool asserver)
        {
            // gameObject.name = next;
            // $"CharacterNameOnChange isServer={asserver}".Log();
        }
     
        
      

        [ServerRpc]
        private void SpawnPrefabServer()
        {
            GameObject go = Instantiate(spherePrefab, transform.position + transform.forward+ new Vector3(0,1,0), Quaternion.identity);
            ServerManager.Spawn(go);
            spawnedObject = go;
            SpawnPrefabObserver( go);
        }
        
        [ObserversRpc]
        private void SpawnPrefabObserver(GameObject go)
        {
            spawnedObject = go;
        }

        [ServerRpc]
        public void DespawnObject()
        {
            if(spawnedObject != null)
                ServerManager.Despawn(spawnedObject);
        }


        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner)
            {
                gameObject.layer = LayerMask.NameToLayer("OtherPlayer");
                 enabled = false;
            } 
            else
            {
                GameManager.Instance.SetPlayer(this);
                GameManager.Instance.SetupAllVirtualCamera();
            }
            
            
            _networkManager = FindObjectOfType<NetworkManager>();
            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager not found, HUD will not function.");
                return;
            }
            else
            {

                if (IsOwner)
                {
                    _networkManager.ClientManager.Connection.ClientId.Log(EColor.RED);
                    _baseCharacterController.isOwner = true;
                    charactername.OnChange+= CharacternameOnOnChange;
                    if (IsServerInitialized)
                    {
                        SetCharacterNameServer("Server");
                    }
                    else
                    {
                        SetCharacterNameServer("Client");
                    }


                }
                // _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
                // _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
            }
        }

       


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                //$"PlayerPresenterAlpha2".Log();
                //DetachTo();
                //GetDamaged(null,KnockBackDirection.RIGHT);
                //DeActiveRagDoll();
                NHealth.Value.Log();
                charactername.Value.Log();
            }
            
            if (!IsOwner)
                return;
            
            playerInteract.FindInteractObject();

            if (player.GetButtonDown("Interact"))
            {
                //Interact();
                PickUp();
            }
            
            if (player.GetButtonDown("Flash"))
            {
                FlashLightToggle();
            }
            
            if (player.GetButtonDown("Crouch"))
            {
                if (!isCrouch)
                {
                    isCrouch = true;
                    ac.crouch = true;
                    ac.speed = 2;
                 
                    "CrouchTrue".Log();
                }
                else
                {
                    isCrouch = false;
                    ac.crouch = false;
                    ac.speed = 5;
                    "CrouchFalse".Log();
                }
              
            }
            

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //$"PlayerPresenterAlpha1".Log();
                //AttachTo(null);
                //GetDamaged(null,KnockBackDirection.LEFT);
                //GetDamaged(null,true);

                //SetHealthServer(NHealth.Value-1);
                //SpawnPrefabServer();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //$"PlayerPresenterAlpha2".Log();
                //DetachTo();
                //GetDamaged(null,KnockBackDirection.RIGHT);
                //DeActiveRagDoll();
                // NHealth.Value.Log();
                // charactername.Value.Log();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //ActiveRagDoll();
                //GetDamaged(null,KnockBackDirection.BACK);
                //SetCharacterNameServer("EJ");
            }
   
            
        }


      


        public void Init()
        {
            isAlive = true;
            ac = GetComponent<AnimeCharacterController>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            _capsuleCollider = GetComponent<CapsuleCollider>();
            player = ReInput.players.GetPlayer(playerID);
            playerInteract = GetComponent<PlayerInteract>();
            _baseCharacterController = GetComponent<BaseCharacterController>();
            _networkAnimator = GetComponent<NetworkAnimator>();
            playerInteract.Init();
            virtualCamera = GameManager.Instance.firstVirtualCamera;
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
            if (!IsOwner)
                return;
            
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

            if (virtualCamera == null)
                return;
            
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
            //_animator.SetTrigger("PickUp");
            _networkAnimator.SetTrigger("PickUp");
            pickupSequence.Restart();
            PickUpObjectServer();
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
            
            
            GameManager.Instance.MMF_CameraShake.PlayFeedbacks();
           
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
            
            
            GameManager.Instance.MMF_CameraShake.PlayFeedbacks();
           
        }

        

        #endregion


        public void ActiveLight()
        {
            characterSpotLight.SetActive(true);
            characterPointLight.SetActive(true);
        }

        public void DeActiveLight()
        {
            
            characterSpotLight.SetActive(false);
            characterPointLight.SetActive(false);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PickUpObjectServer()
        {
            PickUpObjectObserver();
            
        }

        [ObserversRpc(BufferLast = true)]
        public void PickUpObjectObserver()
        {
            Transform PickUpObjectTransform = GameManager.Instance.pickupObjectTest.transform;
            PickUpObjectTransform.parent = pickUpTransform;
            PickUpObjectTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    
    }
}


