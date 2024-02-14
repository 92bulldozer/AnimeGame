
using DarkTonic.MasterAudio;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{

    public void SandWalkSoundEvent()
    {
        MasterAudio.PlaySound3DAtTransform("Sand footsteps 13", transform);
    }
}