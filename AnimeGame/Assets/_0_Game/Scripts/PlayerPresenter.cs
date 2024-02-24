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

        [Header("Field")][Space(10)]
        public bool canInteract;

        public bool isFlashOn;
        public GameObject flashLight;
        public Vector3 moveDirection;

        public float moveSpeed = 1;
        public float maxSpeed = 2;
        public float rotationSpeed = 7;
        public float animationSmoothTime = 0.2f;
        [Header("Component")] [Space(10)] 
        public Animator animator;

        public Camera mainCamera;

        public Transform orientation;

        public Rigidbody rb;


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
            
            
            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
                rb.rotation = Quaternion.Slerp(rb.rotation,lookRotation,rotationSpeed*Time.deltaTime);
                
            }
            else
            {
                rb.angularVelocity=Vector3.zero;
            }

            float characterAnimationSpeed = rb.velocity.magnitude.Remap(0, 2, 0, 1);
            animator.SetFloat("MoveSpeed",characterAnimationSpeed,animationSmoothTime,Time.deltaTime);
            //animator.SetFloat("MoveSpeed",rb.velocity.magnitude,0.1f,Time.deltaTime);
            
        }

        private void FixedUpdate()
        {
            //Debug.Log(rb.velocity.magnitude);
            if(rb.velocity.magnitude <= maxSpeed)
                rb.velocity += moveDirection.normalized * (moveSpeed * 10 * Time.fixedDeltaTime);
            //rb.AddForce(moveDirection.normalized * (moveSpeed *10 * Time.fixedDeltaTime) , ForceMode.Force);
        }
        
        

        public void Init()
        {
            canInteract=true;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            orientation = transform;
            mainCamera = Camera.main;
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
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            Vector3 right = mainCamera.transform.right;
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
    
    }
    
    
    
    
  
}


