using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DarkTonic.MasterAudio;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour, IInteract
{
    public string enterSfx;
    public string clickSfx;
    public bool canInteract = true;
    public UnityEvent interactEvent;
    public bool isInteracted { get; set; }

    [SerializeField] private bool affectOutlinable = false;

    private Outlinable outlinable;

    public virtual void Awake()
    {
        canInteract = true;
    }

    public virtual void Start()
    {
        if (!affectOutlinable)
            return;
        
        outlinable = GetComponent<Outlinable>();
        outlinable.enabled = false;
    }

    public void ResetOutline()
    {
        outlinable.enabled = false;
    }
    public void ResetInteract()
    {
        canInteract = true;
    }

    public virtual void Interact()
    {
        if (!PlayerPresenter.Instance.canInteract || !canInteract)
            return;

     
        canInteract = false;
        isInteracted = !isInteracted;
        

        //AudioSource.PlayClipAtPoint(interactionSound, transform.position, 1.0f);
        MasterAudio.PlaySound3DAtTransform(clickSfx, transform);
        interactEvent?.Invoke();
    }

    public virtual void ShowInteractPanel()
    {
        if (!PlayerPresenter.Instance.canInteract)
            return;
        
        if(affectOutlinable)
            outlinable.enabled = true;


        MasterAudio.PlaySound3DAtTransform(enterSfx, transform);
        PlayerPresenter.Instance.SetInteractObject(this);
        
        
    }

    public virtual void HideInteractPanel()
    {
        if (!PlayerPresenter.Instance.canInteract)
            return;

        PlayerPresenter.Instance.interactableObject = null;

        if(affectOutlinable)
            outlinable.enabled = false;
    }
}