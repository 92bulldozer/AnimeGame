using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour,IInteract
{
    
    [Space(20)] [Header("Field")] [Space(10)]
    public bool canInteract = true;
    public Vector3 panelOffset;
    public Transform interactCenterPosition;
    public Vector3 centerPositionOffset;
    public bool isInteracted { get; set; }
    public bool affectOutlinable = false;
    public bool useReverseInteract = false;
    public bool isReverse = false;
    public EInteractText eInteractText;
    public EInteractText eInteractReverseText;
    public UnityEvent interactEvent;

    private Outlinable outlinable;
    
    public virtual void Awake()
    {
        canInteract = true;
        Init();
    }
    public virtual void Start()
    {
        if (!affectOutlinable)
            return;
        
        outlinable = GetComponent<Outlinable>();
        outlinable.enabled = false;
    }
    
    public virtual void Init()
    {
        try
        {
            interactCenterPosition = transform.GetChild(0).GetComponent<Transform>();

        }
        catch (Exception e)
        {
            interactCenterPosition = transform;
        }
        interactCenterPosition.localPosition += centerPositionOffset;
    }
    
    public void EnableOutline()
    {
        if (affectOutlinable)
        {
            outlinable.enabled = true;
        }
    }

    public void DisableOutline()
    {
        if (affectOutlinable)
        {
            outlinable.enabled = false;
        }
    }
    
    public virtual void Interact()
    {
        //"interact".Log();
        if (isInteracted)
            return;
        
        interactEvent?.Invoke();
        
    }

    public virtual void ShowInteractPanel()
    {
        //"ShowInteractPanel".Log();
        if (useReverseInteract)
        {
            if (isReverse)
                InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractReverseText);
            else
                InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);
            
        }
        else
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);

        }
    }

    public virtual void HideInteractPanel()
    {
        //"HideInteractPanel".Log();
        InteractPresenter.Instance.HideInteractPanel();
    }
    
    public void UpdateReverseText(bool _isReverse)
    {
        if (!_isReverse)
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);
            isReverse=false;
            //"ReverseTrue".Log();
        }
        else
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractReverseText);
            isReverse=true;
            //"ReverseFalse".Log();
        }
    }
}
