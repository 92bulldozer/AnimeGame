using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using EJ;
using Pathfinding;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour
{
    [Header("Field")] [Space(10)] 
    public bool isPatrolling;
    
    [Header("Component")] [Space(10)]
    public Animator animator;

    public AIPath aiPath;
    
    [Header("BehaviorTree")][Space(10)]
    public List<BehaviorTree> behaviorTreeList;
    
    public float MaxSpeed { get; set; }
    public float FollowSpeed { get; set; }
    public GameObject targetObject { get; set; }

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
    }

    public void UpdateLocomotion()
    {
        animator.SetFloat("Speed",aiPath.velocity.magnitude / aiPath.maxSpeed,0.2f,Time.deltaTime);
        
        // if(isPatrolling)
        //     animator.SetFloat("Speed",aiPath.velocity.magnitude / aiPath.maxSpeed,0.1f,Time.deltaTime);
        // else
        // {
        //     animator.SetFloat("Speed",0,0.1f,Time.deltaTime);
        // }
    }
}
