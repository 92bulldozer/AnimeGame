using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;

public class InteractSender : MonoBehaviour
{
    public IInteract iInteract;
    public Vector3 panelOffset;
    public string interactText;
    public string interactReverseText;

    private void Awake()
    {
        iInteract = transform.parent.GetComponent<IInteract>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && iInteract != null)
        {
            "ShowInteractPanel".Log();

            if (!iInteract.isInteracted)
                InteractPresenter.Instance.ShowInteractPanel(transform.parent,panelOffset,interactText);
            else
                InteractPresenter.Instance.ShowInteractPanel(transform.parent,panelOffset,interactReverseText);
            
            iInteract.ShowInteractPanel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player")&& iInteract != null)
        {
            "HideInteractPanel".Log();
            InteractPresenter.Instance.HideInteractPanel();
            iInteract.HideInteractPanel();
            
        }
    }

    public void UpdateReverseText()
    {
        if (!iInteract.isInteracted)
        {
            "열기텍스트".Log();
            InteractPresenter.Instance.ShowInteractPanel(transform.parent,panelOffset,interactText);
           
        }
        else
        {
            "닫기텍스트".Log();
            InteractPresenter.Instance.ShowInteractPanel(transform.parent,panelOffset,interactReverseText);
        }
    }
}
