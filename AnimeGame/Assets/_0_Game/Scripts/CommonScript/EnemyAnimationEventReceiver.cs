
using System;
using AnimeGame;
using DarkTonic.MasterAudio;
using FIMSpace.FProceduralAnimation;
using UnityEngine;

public class EnemyAnimationEventReceiver : MonoBehaviour 
{

    
    public string sfx;
    public string sprintSfx;
    public string attackSfx;
    public EnemyPresenter enemyPresenter;

    private void Awake()
    {
        enemyPresenter = transform.root.GetComponent<EnemyPresenter>();
    }

    public void FootStepSound(int stepIdx)
    {
        if (enemyPresenter.IsSprint)
        {
            if (stepIdx == 1)
                MasterAudio.PlaySound3DAtTransform(sprintSfx,transform);
        }
        else
        {
            if(stepIdx==0)
                MasterAudio.PlaySound3DAtTransform(sfx,transform);
        }
    }

    public void AttackSound()
    {
        MasterAudio.PlaySound3DAtTransform(attackSfx,transform);
    }

    public void EnemyAttack(int direction)
    {
        PlayerPresenter.Instance.GetDamaged(transform,(KnockBackDirection)direction);

    }
    
    
   
   

  
}