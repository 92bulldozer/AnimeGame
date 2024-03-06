using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using DarkTonic.MasterAudio;
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
    [Header("Sfx")] [Space(10)] 
    public string monsterHowl;
    

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
        LoopHowl();
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

    public void LoopHowl()
    {
        StartCoroutine(LoopHowlCor());
    }

    IEnumerator LoopHowlCor()
    {
        while (true)
        {
            //MasterAudio.PlaySound3DAtTransform(monsterHowl, transform);

            yield return new WaitForSeconds(6);
        }
    }

    public void SetLegAnimator()
    {
        
    }
}
