
using System;
using AnimeGame;
using DarkTonic.MasterAudio;
using EJ;
using RootMotion.FinalIK;
using UnityEngine;

public class EnemyAnimationEventReceiver : MonoBehaviour 
{
    public string sfx;
    public string sprintSfx;
    public string attackSfx;
    public EnemyPresenter enemyPresenter;
    public Transform attachTransform;
    public FullBodyBipedIK fbbik;
    public bool useLeftHandIK;

    private void Awake()
    {
        enemyPresenter = transform.parent.GetComponent<EnemyPresenter>();
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
        GameManager.Instance.playerPresenter.GetDamaged(transform,(KnockBackDirection)direction);

    }
    
    public void EnemyAttackOnlyBlood(int direction)
    {
        GameManager.Instance.playerPresenter.GetDamagedOnlyBlood(transform,(KnockBackDirection)direction);

    }

    public void GrabPlayer()
    {
        GameManager.Instance.playerPresenter.AttachTo(attachTransform);
    }
    
    public void ReleasePlayer()
    {
        GameManager.Instance.playerPresenter.DetachTo();
    }

    public void EnableFBBIK()
    {
        fbbik.solver.leftHandEffector.positionWeight = 1;
        fbbik.solver.leftHandEffector.rotationWeight = 1;
    }
    
    public void DisableFBBIK()
    {
        fbbik.solver.leftHandEffector.positionWeight = 0;
        fbbik.solver.leftHandEffector.rotationWeight = 0;
    }
    
    
   
   

  
}