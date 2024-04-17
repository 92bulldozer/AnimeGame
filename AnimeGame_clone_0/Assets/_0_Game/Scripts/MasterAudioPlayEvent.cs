using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class MasterAudioPlayEvent : MonoBehaviour
{
    public void PlaySound(string name)
    {
        MasterAudio.PlaySound(name);
    }
}
