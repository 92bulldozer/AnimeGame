using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Doozy.Engine;
using EJ;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AnimeGame
{
    public class PlayerPresenter : MonoBehaviour
    {
        public static PlayerPresenter Instance;

        [Header("Field")] [Space(10)] 
        public bool canInteract = false;
        public bool isFlashOn;
        public Vector3 moveDirection;
        public float moveSpeed = 1;
        public float maxSpeed = 2;
        public float rotationSpeed = 7;
        public float animationSmoothTime = 0.2f;
        
        [Header("Component")] [Space(10)] 
        public List<Collider> ragDollColliderList;
        public List<Rigidbody> ragDollRigidBodyList;
        public List<float> ragDollMassList;
        public GameObject flashLight;
        public InteractableObject interactableObject;
        public GameObject virtualCamera;



        private Camera _mainCamera;
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rb;
        public Animator _animator;

        [Header("Sfx")] [Space(10)] 
        public string footStepSfx;

        [Header("Stair")] [Space(10)] 
        public GameObject stepRayLower;
        public GameObject stepRayUpper;
        public float stepSmooth;
        public float rayLength;
        public float stepUpForce;
        public float stepUpOffset;
        private int stepLayer;

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
            
            if(Input.GetKeyDown(KeyCode.Alpha1))
                ActiveRagDoll();
            
            if(Input.GetKeyDown(KeyCode.E))
                Interact();
            
            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
                _rb.rotation = Quaternion.Slerp(_rb.rotation,lookRotation,rotationSpeed*Time.deltaTime);
                
            }
            else
            {
                _rb.angularVelocity=Vector3.zero;
            }

            float characterAnimationSpeed = _rb.velocity.magnitude.Remap(0, 2, 0, 1);
            _animator.SetFloat("MoveSpeed",characterAnimationSpeed,animationSmoothTime,Time.deltaTime);
            //animator.SetFloat("MoveSpeed",rb.velocity.magnitude,0.1f,Time.deltaTime);
            
        }

        private void FixedUpdate()
        {
            //Debug.Log(rb.velocity.magnitude);
            if(_rb.velocity.magnitude <= maxSpeed)
                _rb.velocity += moveDirection.normalized * (moveSpeed * 10 * Time.fixedDeltaTime);
            //rb.AddForce(moveDirection.normalized * (moveSpeed *10 * Time.fixedDeltaTime) , ForceMode.Force);
            
            StepClimb();
        }
        
        

        public void Init()
        {
            canInteract=true;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            _capsuleCollider = GetComponent<CapsuleCollider>();
            foreach (var collider in ragDollColliderList)
            {
                //collider.enabled = false;
                collider.isTrigger = true;
            }
            foreach (var rigidbody in ragDollRigidBodyList)
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                ragDollMassList.Add(rigidbody.mass);
            }

            stepLayer = (1 << LayerMask.NameToLayer("Default"));
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

        public void OnMove(InputValue _inputValue)
        {
            Vector2 inputValue = _inputValue.Get<Vector2>();
            
            //moveDirection = orientation.forward * inputValue.x + orientation.right * inputValue.y;
            Vector3 forward = _mainCamera.transform.forward;
            forward.y = 0;
            Vector3 right = _mainCamera.transform.right;
            right.y = 0;
            moveDirection = forward * inputValue.y + right * inputValue.x;
            
            
        }
        
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
            _animator.enabled = false;
            _capsuleCollider.enabled = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            foreach (var collider in ragDollColliderList)
            {
                //collider.enabled = true;
                collider.isTrigger = false;
            }
            foreach (var rigidbody in ragDollRigidBodyList)
            {
                rigidbody.isKinematic = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }


            MasterAudio.PlaySound3DAtTransform("FemaleScream", transform);
            "PlayerRagDollActive".Log();
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

        public void StepClimb()
        {
            Debug.DrawRay(stepRayLower.transform.position,transform.forward,Color.blue,0.1f);
            
            RaycastHit hitLower;
            if (Physics.Raycast(stepRayLower. transform.position, transform.forward,
                    out hitLower, rayLength,stepLayer))
            {
                RaycastHit hitUpper2;
                if (Physics.Raycast(stepRayUpper.transform.position + new Vector3(0, stepUpOffset, 0),
                        transform.forward,
                        out hitUpper2, rayLength, stepLayer))
                {
                    "리턴".Log();
                    return;
                }
                
                RaycastHit hitUpper;
                if (Physics.Raycast(stepRayUpper.transform.position, transform.forward,
                        out hitUpper, rayLength,stepLayer))
                {
                    _rb.position += (new Vector3(0, stepSmooth, 0) + moveDirection.normalized * stepUpForce) *
                                    Time.fixedDeltaTime;
                    _rb.useGravity = false;
                    return;
                }
              
               
            }
            else
                _rb.useGravity = true;
            
        
            
            Debug.DrawRay(stepRayLower.transform.position,(transform.forward-transform.right).normalized,Color.red,0.1f);
            RaycastHit hitLower45;
            if (Physics.Raycast(stepRayLower. transform.position, (transform.forward-transform.right).normalized,
                    out hitLower45, rayLength*2,stepLayer))
            {
                RaycastHit hitUpper452;
                if (Physics.Raycast(stepRayUpper.transform.position + new Vector3(0, stepUpOffset, 0),
                        (transform.forward-transform.right).normalized,
                        out hitUpper452, rayLength*2, stepLayer))
                {
                    "리턴".Log();
                    return;
                }
                
                RaycastHit hitUpper45;
                if (Physics.Raycast(stepRayUpper.transform.position, (transform.forward-transform.right).normalized,
                        out hitUpper45, rayLength*2,stepLayer))
                {
                    _rb.position += (new Vector3(0, stepSmooth, 0) + moveDirection.normalized * stepUpForce) *2*
                                    Time.fixedDeltaTime;
                    _rb.useGravity = false;
                    $"L {hitUpper45.transform.name}".Log();
                    return;
                }
              
            }
            else
                _rb.useGravity = true;
            
            
            
            
            Debug.DrawRay(stepRayLower.transform.position,(transform.forward+transform.right).normalized,Color.yellow,0.1f);
            
            RaycastHit hitLowerminus45;
            if (Physics.Raycast(stepRayLower. transform.position, (transform.forward+transform.right).normalized,
                    out hitLowerminus45, rayLength*2,stepLayer))
            {
                RaycastHit hitUpperminus2;
                if (Physics.Raycast(stepRayUpper.transform.position + new Vector3(0, stepUpOffset, 0),
                        (transform.forward+transform.right).normalized,
                        out hitUpperminus2, rayLength*2, stepLayer))
                {
                    "리턴".Log();
                    return;
                }
                
                RaycastHit hitUpperminus45;
                if (Physics.Raycast(stepRayUpper.transform.position, (transform.forward+transform.right).normalized,
                        out hitUpperminus45, rayLength*2,stepLayer))
                {
                    _rb.position += (new Vector3(0, stepSmooth, 0) + moveDirection.normalized * stepUpForce) *2*
                                    Time.fixedDeltaTime;
                    _rb.useGravity = false;
                    $"R {hitUpperminus45.transform.name}".Log();
                    return;
                }
              
            }
            else
                _rb.useGravity = true;
            
            
            
            
        }


       
        
        
    
    }
}


