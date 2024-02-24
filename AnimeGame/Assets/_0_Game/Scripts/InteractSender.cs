using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;

public class InteractSender : MonoBehaviour
{
    public IInteract iInteract;
    public Vector3 panelOffset;

    private void Awake()
    {
        iInteract = transform.parent.GetComponent<IInteract>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && iInteract != null)
        {
            "ShowInteractPanel".Log();
            InteractPresenter.Instance.ShowInteractPanel(transform.parent,panelOffset);
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
}
