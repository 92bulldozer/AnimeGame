using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class VfxSound : MonoBehaviour
{
    public string sfxName;
    public float loopDelayTime;
    public int maxLooptCount;
    public bool useLoopSound;
    private WaitForSeconds loopDelayWs;

    private void Awake()
    {
        loopDelayWs = new WaitForSeconds(loopDelayTime);
    }

    public void PlaySound()
    {
        if (sfxName.Equals("") || sfxName == null)
            return;
        
        if(!useLoopSound)
            MasterAudio.PlaySound(sfxName);
        else
            StartCoroutine(LoopCor());
        
    }
    

    IEnumerator LoopCor()
    {
        int loopCount = 0;
        while (true)
        {
            if(loopCount==maxLooptCount)
                yield break;

            loopCount++;
            MasterAudio.PlaySound(sfxName);

            yield return loopDelayWs;
        }
    }
}
