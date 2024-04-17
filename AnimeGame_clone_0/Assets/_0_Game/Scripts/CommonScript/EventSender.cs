using System;
using System.Collections;
using Doozy.Engine;
using UnityEngine;

public class EventSender : MonoBehaviour
{
    public string eventName;
    public bool onStart;
    public bool useDelay;
    public float delayTime;

    private WaitForSeconds delayWs;

    private void Awake()
    {
        delayWs = new WaitForSeconds(delayTime);
    }

    private void Start()
    {
        if(onStart)
            SendEvent();
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
