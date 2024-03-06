
using AnimeGame;
using DarkTonic.MasterAudio;
using FIMSpace.FProceduralAnimation;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour 
{

    
    public string sfx;

    public void FootStepSound()
    {
        MasterAudio.PlaySound3DAtTransform(sfx,transform);
    }
    
  

    public void EnemyAttack()
    {
        PlayerPresenter.Instance.GetDamaged(transform,true);

    }
    
    public void SandWalkSoundEvent()
    {
    }

  
}