using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class SoundEventReceiver : MonoBehaviour
{
    public string sfx;
    public string sprintSfx;

    public void PlaySound()
    {
        MasterAudio.PlaySound3DAtTransform(sfx,transform);
    }
    
    public void PlaySprintSound()
    {
        MasterAudio.PlaySound3DAtTransform(sprintSfx,transform);
    }
}
