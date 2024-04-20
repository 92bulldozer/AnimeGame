using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using BehaviorDesigner.Runtime;
using DarkTonic.MasterAudio;
using EJ;
using FishNet.Object;
using Pathfinding;
using UnityEngine;

public class EnemyPresenter : NetworkBehaviour
{
    [Header("Field")] [Space(10)] 
    public bool isPatrolling;
    public bool useSprintAnimation;
    public float interactRange = 1;
    public int layerMask;
    private WaitForSeconds checkInteractableObjectWs;
    
    [Header("Component")] [Space(10)]
    public Animator animator;

    public AIPath aiPath;
    
    [Header("BehaviorTree")][Space(10)]
    public List<BehaviorTree> behaviorTreeList;
    
    public float MaxSpeed { get; set; }
    public float FollowSpeed { get; set; }
    public bool IsSprint { get; set; }
    public GameObject targetObject { get; set; }
    public bool isRandomPatrol { get; set; }
    public List<GameObject> wayPointList { get; set; }
    //[Header("Sfx")] [Space(10)] 
    
    
    private Collider[] interactColliderArray;
    

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        StartCoroutine(CheckInteractableObject());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    private void Update()
    {
        
        UpdateLocomotion();
    }

    private void Init()
    {
        animator = GetComponentInChildren<Animator>();
        aiPath = GetComponent<AIPath>();
        isPatrolling = true;
        wayPointList = new List<GameObject> { WayPointManager.Instance.SelectRandomWayPoint() };
        checkInteractableObjectWs = new WaitForSeconds(3f);
        interactColliderArray = new Collider[10];
        layerMask = 1 << LayerMask.NameToLayer("InteractableObject");
        isRandomPatrol = true;
    }

    public void UpdateLocomotion()
    {
        if (useSprintAnimation)
        {
            if(!IsSprint)
                animator.SetFloat("Speed",(aiPath.velocity.magnitude / aiPath.maxSpeed)*0.3f,0.2f,Time.deltaTime);
            else
                animator.SetFloat("Speed",aiPath.velocity.magnitude / aiPath.maxSpeed,0.2f,Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed",aiPath.velocity.magnitude / aiPath.maxSpeed,0.2f,Time.deltaTime);
        }
     
    }

    IEnumerator CheckInteractableObject()
    {
        while (true)
        {
            Array.Clear(interactColliderArray, 0, 10);
            var size = Physics.OverlapSphereNonAlloc(transform.position, interactRange, interactColliderArray,layerMask);
            GameObject shortDistanceObject = null;
            //currentInteractableObject = null;
            float shortDistance = 1000000000;

            for (int i = 0; i < size; i++)
            {
                float distance = Vector3.Distance(gameObject.transform.position, interactColliderArray[i].transform.position);

                if (distance < shortDistance)
                {
                    shortDistanceObject = interactColliderArray[i].gameObject;
                    shortDistance = distance;
                
                
                }
            }
            
            if (size == 0)
            {
               //"size =0".Log();
            }
            else
            {
                InteractableObject io = shortDistanceObject.GetComponent<InteractableObject>();

                try
                {
                    io.Interact();
                    "InteractObjectEnemy".Log();

                }
                catch (Exception e)
                {
                    
                }
            
            }
            
            
            
            yield return checkInteractableObjectWs;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractableObject"))
        {
            if (other.TryGetComponent(out InteractableObject io))
            {
                "InteractObjectEnemy".Log();
                io.Interact();
            }
        }
    }
}
