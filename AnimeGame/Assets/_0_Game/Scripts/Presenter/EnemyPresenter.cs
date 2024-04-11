using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using BehaviorDesigner.Runtime;
using DarkTonic.MasterAudio;
using EJ;
using Pathfinding;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour
{
    [Header("Field")] [Space(10)] 
    public bool isPatrolling;
    public bool useSprintAnimation;
    
    [Header("Component")] [Space(10)]
    public Animator animator;

    public AIPath aiPath;
    
    [Header("BehaviorTree")][Space(10)]
    public List<BehaviorTree> behaviorTreeList;
    
    public float MaxSpeed { get; set; }
    public float FollowSpeed { get; set; }
    public bool IsSprint { get; set; }
    public GameObject targetObject { get; set; }
    public List<GameObject> wayPointList { get; set; }
    //[Header("Sfx")] [Space(10)] 
    

    private void Awake()
    {
        Init();
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
        wayPointList = WayPointManager.Instance.wayPointList;
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

    

}
