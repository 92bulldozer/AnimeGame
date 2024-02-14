using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Doozy.Engine.UI;
using EJ;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpGradeBtn : MonoBehaviour
{
    [SerializeField] private bool isPointerDown;
    public float delayTime = 0.07f;
    public bool useScrolling;
    public ScrollRect scrollRect;
    
    private UIButton uiButton;
    private WaitForSeconds _delayTimeWs;

    public UnityEvent upgradeSuccessCallback;
    

    
    public bool IsPointerDown
    {
        get => isPointerDown;
        set => isPointerDown = value;
    }
    
    
    private void Awake()
    {
        uiButton = GetComponent<UIButton>();
        _delayTimeWs = new WaitForSeconds(delayTime);
    }

    private void Start()
    {
        uiButton.OnPointerDown.Enabled = true;
        uiButton.OnPointerUp.Enabled = true;
        uiButton.OnClick.Enabled = true;
        uiButton.OnLongClick.Enabled = true;
        
        uiButton.OnPointerDown.OnTrigger.Event.AddListener(()=>
        {
            IsPointerDown = true;
            try
            {
                if(useScrolling)
                    scrollRect.enabled = false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
        });
        uiButton.OnPointerUp.OnTrigger.Event.AddListener(()=>
        {
            IsPointerDown = false;
            try
            {
                if(useScrolling)
                    scrollRect.enabled = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
        });
        
        uiButton.OnClick.OnTrigger.Event.AddListener(()=>
        {
            OnClickUpGrade();
        });
        
        uiButton.OnLongClick.OnTrigger.Event.AddListener(()=>
        {
            LongClickUpGrade();
        });
    }

    public void OnClickUpGrade()
    {
        upgradeSuccessCallback?.Invoke();
    }
    
    public void LongClickUpGrade()
    {
        isPointerDown = true;
        "LongClick".Log();
        StartCoroutine(LongClickUpGradeCor());
    }

    IEnumerator LongClickUpGradeCor()
    {
        while (isPointerDown)
        {
            upgradeSuccessCallback?.Invoke();
            yield return _delayTimeWs;
        }
    }
}
