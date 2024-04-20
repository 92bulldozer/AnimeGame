using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Prediction;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

namespace AnimeGame
{


    public class FishNetPredictPlayer : NetworkBehaviour
    {
        public struct MoveData : IReplicateData
        {
            public bool Jump;
            public float Horizontal;
            public float Vertical;
            public MoveData(bool jump, float horizontal, float vertical)
            {
                Jump = jump;
                Horizontal = horizontal;
                Vertical = vertical;
                _tick = 0;
            }

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }

        public struct ReconcileData : IReconcileData
        {
            //As of 4.1.3 you can use RigidbodyState to send
            //the transform and rigidbody information easily.
            public RigidbodyState RigidbodyState;
            //As of 4.1.3 PredictionRigidbody was introduced.
            //It primarily exists to create reliable simulations
            //when interacting with triggers and collider callbacks.
            public PredictionRigidbody PredictionRigidbody;
    
            public ReconcileData(PredictionRigidbody pr)
            {
                RigidbodyState = new RigidbodyState(pr.Rigidbody);
                PredictionRigidbody = pr;
                _tick = 0;
            }

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }
        public override void OnStartNetwork()
        {
            base.TimeManager.OnTick += TimeManager_OnTick;
            base.TimeManager.OnPostTick += CreateReconcile;
        }

        public override void OnStopNetwork()
        {
            base.TimeManager.OnTick -= TimeManager_OnTick;
            base.TimeManager.OnPostTick -= CreateReconcile;
        }
        
        private void TimeManager_OnTick()
        {
            Move(BuildMoveData());
        }

        public override void CreateReconcile()
        {
            if (IsServerStarted)
            {
                ReconcileData rd = new ReconcileData(PredictionRigidbody);
                //if (!base.IsOwner)
                //    Debug.LogError($"Frame {Time.frameCount}. Reconcile, MdTick {LastMdTick}, PosX {transform.position.x.ToString("0.##")}. VelX {Rigidbody.velocity.x.ToString("0.###")}.");
                Reconciliation(rd);
            }
        }

        [Reconcile]
        private void Reconciliation(ReconcileData rd, Channel channel = Channel.Unreliable)
        {
            //Sets state of transform and rigidbody.
            Rigidbody rb = PredictionRigidbody.Rigidbody;
            rb.SetState(rd.RigidbodyState);
            //Applies reconcile information from predictionrigidbody.
            PredictionRigidbody.Reconcile(rd.PredictionRigidbody);
        }
        
        [SerializeField]
        private float _jumpForce = 15f;
        [SerializeField]
        private float _moveRate = 15f;

        private PredictionRigidbody PredictionRigidbody { get;  set; } = new();
        private bool _jump;
        
        private void Awake()
        {
            PredictionRigidbody.Initialize(GetComponent<Rigidbody>());
        }

        private void Update()
        {
            if (base.IsOwner)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    _jump = true;
            }
        }
        
       

        private MoveData BuildMoveData()
        {
            if (!base.IsOwner)
                return default;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            MoveData md = new MoveData(_jump, horizontal, vertical);
            _jump = false;

            return md;
        }
        
        [Replicate]
        private void Move(MoveData md, ReplicateState state = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
        {
            /* ReplicateState is set based on if the data is new, being replayed, ect.
            * Visit the ReplicationState enum for more information on what each value
            * indicates. At the end of this guide a more advanced use of state will
            * be demonstrated. */
            Vector3 forces = new Vector3(md.Horizontal, 0f, md.Vertical) * _moveRate;
            PredictionRigidbody.AddForce(forces);

            if (md.Jump)
            {
                Vector3 jmpFrc = new Vector3(0f, _jumpForce, 0f);
                PredictionRigidbody.AddForce(jmpFrc, ForceMode.Impulse);
            }
            //Add gravity to make the object fall faster.
            PredictionRigidbody.AddForce(Physics.gravity * 3f);
            //Simulate the added forces.
            PredictionRigidbody.Simulate();
        }
    }
}