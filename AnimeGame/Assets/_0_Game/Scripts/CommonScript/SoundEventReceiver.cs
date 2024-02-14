using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class SoundEventReceiver : MonoBehaviour
{
    public string sfx;

    public void PlaySound()
    {
        MasterAudio.PlaySound(sfx);
    }
}
