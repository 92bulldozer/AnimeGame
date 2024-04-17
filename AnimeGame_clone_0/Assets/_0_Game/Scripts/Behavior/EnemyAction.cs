using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using EJ;
using UnityEngine;

public class EnemyAction : Action
{
    protected Animator animator;
    protected EnemyPresenter enemyPresenter;
    public SharedString animationTriggerName;

    
    
    public override void OnAwake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        enemyPresenter = gameObject.GetComponent<EnemyPresenter>();
    }

   

    
}
