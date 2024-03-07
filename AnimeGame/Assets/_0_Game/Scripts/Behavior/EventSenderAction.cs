using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Doozy.Engine;
using UnityEngine;

public class EventSenderAction : Action
{
    public string eventName;
    public bool useDelay;
    public float delayTime;
    public bool isFirstSkip;

    private WaitForSeconds delayWs;
    
    public override void OnAwake()
    {
        base.OnAwake();
        delayWs = new WaitForSeconds(delayTime);
    }

    public override void OnStart()
    {
        base.OnStart();
        
        if(!isFirstSkip)
            SendEvent();
        
        isFirstSkip = false;
    }
    
    public void SendEvent()
    {
        if (useDelay)
            StartCoroutine(DelaySendEvent());
        else
            GameEventMessage.SendEvent(eventName);

        
    }

    IEnumerator DelaySendEvent()
    {
        yield return delayWs;
        GameEventMessage.SendEvent(eventName);
    }
}
