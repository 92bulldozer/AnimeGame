using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using EJ;
using Pathfinding;
using UnityEngine;

[TaskCategory("AnimeGame")]
public class EnemyAttack : EnemyAction
{
    public bool isAttackEnd;
    public float animationTime = 1;
    
    public override void OnStart()
    {
        isAttackEnd = false;
        animator.SetTrigger(animationTriggerName.Value);
        enemyPresenter.isPatrolling = false;
        GetComponent<AIPath>().StopAI();
        GameManager.Instance.playerPresenter.DisableInput();
        DOVirtual.DelayedCall(animationTime, () =>
        {
            isAttackEnd = true;
        });
    }

    public override TaskStatus OnUpdate()
    {
        if (isAttackEnd)
        {
            return TaskStatus.Success;

        }
        else
        {
            return TaskStatus.Running;

        }
    }

    public override void OnEnd()
    {
    }
}
